using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;

namespace DBS.Circulares.Event_Receivers.CircularesEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class CircularesEventReceiver : SPItemEventReceiver
    {
       /// <summary>
       /// An item was added.
       /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
       {
           replicarItem(properties);
           base.ItemAdded(properties);
       }

        /// <summary>
        /// Recebe o item que foi adicionado e copia o item para cada nova pessoa que deve receber
        /// </summary>
        /// <param name="properties"></param>
        private void replicarItem(SPItemEventProperties properties)
        {
            using (SPWeb web = properties.Web.Site.RootWeb)
            {
                try
                {
                    web.AllowUnsafeUpdates = true;
                    SPFieldUserValueCollection usuarios = (SPFieldUserValueCollection)properties.ListItem["Destinatario"];
                    List<SPUser> SPusuarios = capturarTodosUsuarios(usuarios, properties);
                    //usuarios = capturarTodosUsuarios(usuarios, properties);

                    foreach (SPUser usuario in SPusuarios)
                    {
                        SPList lstCircularesEnviadas = web.Lists["DBS.CircularesEnviadas"];
                        SPListItem novoItem;

                        novoItem = lstCircularesEnviadas.Items.Add();

                        SPAttachmentCollection anexos = properties.ListItem.Attachments;

                        novoItem["Title"] = (String)properties.ListItem["Title"];
                        novoItem["Descricao"] = properties.ListItem["Mensagem"];
                        novoItem["Destinatario"] = usuario;
                        novoItem["EnviarEmail"] = (bool)properties.ListItem["EnviarEmail"];
                        novoItem["Ciente"] = false;
                        novoItem["DataHoraCiente"] = null;
                        novoItem.Update();  //Para poder incluir anexos, é necessário ter salvo previamente o novo item

                        //Copiando os anexos:
                        if (anexos.Count > 0)
                        {
                            //get the folder with the attachments for the source item
                            SPFolder sourceItemAttachmentsFolder =
                                properties.ListItem.Web.Folders["Lists"].SubFolders[properties.ListItem.ParentList.Title].SubFolders["Attachments"].SubFolders[properties.ListItem.ID.ToString()];
                            //Loop over the attachments, and add them to the target item
                            foreach (SPFile file in sourceItemAttachmentsFolder.Files)
                            {
                                byte[] binFile = file.OpenBinary();
                                novoItem.Attachments.AddNow(file.Name, binFile);
                            }
                            novoItem.Update();                           
                        }

                        //Envio o e-mail
                        EnviarEmail(web, usuario, properties.ListItem);
                        novoItem["EmailEnviado"] = true;
                        novoItem.Update();

                    }
                }
                catch (Exception ex)
                {
                    //todo: salvar em algum lugar essa exception  
                    throw ex;
                }
                finally
                {
                    web.AllowUnsafeUpdates = false;
                }
            }
        }

        private void EnviarEmail(SPWeb web, SPUser destinatario, SPListItem item)
        {
            String corpoMensagem = destinatario.Name + ", você tem uma nova circular para ser lida. Acesse o portar intranet e veja ";

            SPUtility.SendEmail(web, true, true, destinatario.Email, "Nova Circular - " + item["Title"].ToString(), corpoMensagem);
        }

        /// <summary>
        /// Recebe a lista de destinatários com usuários e grupos e retorna uma lista com os usuários envolvidos (cada usuário
        /// de cada grupo), sem repetição e sem os grupos.
        /// </summary>
        /// <param name="listaDestinatarios"></param>
        /// <returns>Usuários sem os grupos</returns>
        private List<SPUser> capturarTodosUsuarios(SPFieldUserValueCollection listaDestinatarios, SPItemEventProperties properties)
        {
            List<SPUser> listaRetorno = new List<SPUser>();
            List<int> usuariosListaRetorno = new List<int>();
            using (SPWeb web = properties.Web.Site.RootWeb)
            {               
                List<SPUser> usuariosDeGrupos = new List<SPUser>();

                SPFieldUserValueCollection collectionGrupos = new SPFieldUserValueCollection();
                
                foreach (SPFieldUserValue usuarioOuGrupo in listaDestinatarios)
                {
                    //jogo grupos em outra lista
                    if (usuarioOuGrupo.User==null)             //Ver se vou precisar colocar a linha if(SPUtility.IsLoginValid(site, usersField.User.LoginName))
                    {
                        collectionGrupos.Add(usuarioOuGrupo);
                    }
                    else  //Adiciona o usuário caso já não esteja na lista
                    {
                        if (!usuariosListaRetorno.Contains(usuarioOuGrupo.User.ID))    
                        {
                            usuariosListaRetorno.Add(usuarioOuGrupo.User.ID);
                            listaRetorno.Add(usuarioOuGrupo.User);
                        }
                    }
                }                

                //Se tem grupos, devo separar cada usuário
                if (collectionGrupos.Count > 0)
                {
                    foreach (SPFieldUserValue grupo in collectionGrupos)
                    {
                        SPGroup group = web.Groups.GetByID(grupo.LookupId);

                        foreach (SPUser user in group.Users)
                        {
                            if (!usuariosDeGrupos.Contains(user))
                            {
                                // add all the group users to the list
                                usuariosDeGrupos.Add(user);
                            }
                        }
                    }
                }

                foreach (SPUser user in usuariosDeGrupos)
                {
                    if (!usuariosListaRetorno.Contains(user.ID))
                    {
                        // add all the group users to the list
                        usuariosListaRetorno.Add(user.ID);
                        listaRetorno.Add(user);
                    }
                }
            }

            return listaRetorno;
        }

       /// <summary>
       /// An item was updated.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
           base.ItemUpdated(properties);
       }

       /// <summary>
       /// An item was deleted.
       /// </summary>
       public override void ItemDeleted(SPItemEventProperties properties)
       {
           base.ItemDeleted(properties);
       }
    }
}
