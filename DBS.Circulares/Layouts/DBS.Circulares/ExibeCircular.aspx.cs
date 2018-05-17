using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Collections;

namespace DBS.Circulares.Layouts.DBS.Circulares
{
    public partial class ExibeCircular : LayoutsPageBase
    {
        private int idCircularEnviada;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                idCircularEnviada = int.Parse(Request.QueryString["ID"]);
                
                carregaCamposNaTela();               
            }
            catch (Exception ex)
            {
                lblErro.Text = ex.ToString();
            }
        }

        private void carregaCamposNaTela()
        {
            

            using (SPWeb web = SPContext.Current.Site.RootWeb)
            {
                SPList lstCircularesEnviadas = web.Lists["DBS.CircularesEnviadas"];
                SPListItem circular = lstCircularesEnviadas.GetItemById(idCircularEnviada);

                lblTitulo.Text = ValorString(circular, "Title");
                lblDescricao.Text = ValorString(circular,"Descricao");
                lblCiente.Text = ValorBool(circular, "Ciente") == true ? "Sim" : "Não";
                lblCriadoEm.Text = ValorString(circular, "Created");
                lblLinks.Text =  montaLinks(circular);
            }
        }

        private String montaLinks(SPListItem item)
        {
            StringBuilder sb = new StringBuilder();

            string listUrl = item.Web.Url + "/" + item.ParentList.RootFolder.Url;
            string attachmentUrl = listUrl + "/attachments/" + item.ID + "/";
           
            foreach (String nomeArquivo in item.Attachments)
            {
                sb.AppendLine("<a href= \"" + attachmentUrl + nomeArquivo + "\">" + nomeArquivo + "</a> <br/>");             
            }
            return sb.ToString();
        }

        private void marcarCiente()
        {
            idCircularEnviada = int.Parse(Request.QueryString["ID"]);
            SPWeb web = SPContext.Current.Site.RootWeb;
            //using (SPWeb web = SPContext.Current.Site.RootWeb)
            //{                
            try
            {
                SPList lstCircularesEnviadas = web.Lists["DBS.CircularesEnviadas"];
                SPListItem circular = lstCircularesEnviadas.GetItemById(idCircularEnviada);
                web.AllowUnsafeUpdates = true;
                circular["Ciente"] = true;
                circular["DataHoraCiente"] = DateTime.Now;
                circular.Update();
                btnMarcarCiente.Enabled = false;
                lblCiente.Text = "Sim";
            }
            catch (Exception ex)
            {
                lblErro.Text = ex.ToString();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }

            //}
        }

        protected void btnFechar_Click(object sender, EventArgs e)
        {
            if (SPContext.Current.IsPopUI)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "CloseForm();", true);
            }
        }

        protected void btnMarcarCiente_Click(object sender, EventArgs e)
        {
            marcarCiente();
        }
        
        private String ValorString(SPListItem item,String campo )
        {
            return item[campo] != null ? item[campo].ToString() : "";
        }

        /// <summary>
        /// Retornar true ou false para valores booleanos. Caso o campo esteja vazio, retorna false
        /// </summary>
        /// <param name="item"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        private bool ValorBool(SPListItem item, String campo)
        {
            bool resultado = false;
            if (item[campo] != null)
            {
                resultado = bool.Parse(item[campo].ToString());
            }
            return resultado;
        }

    }
}
