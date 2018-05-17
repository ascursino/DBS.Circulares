/* 
                          Desenvolvido por DBS IT Services
                         http://www.dbsitservices.com.br
                                     
           Copyright 2012 DBS IT Services © Todos os direitos reservados 

 Este aquivo contém código fonte de aplicativo desenvolvido pela DBS IT Services. 
 É expressamente proibida a alteração, distribuição ou venda desses arquivos sem 
 aprovação  formal da DBS IT Services e do cliente contratante desse serviço sob
 proteção da legislação vigente.
 
*/

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Microsoft.SharePoint.WebPartPages;

namespace DBS.Circulares.Visual_Web_Parts.ExibeCircularesPorPessoa
{    
    public partial class ExibeCircularesPorPessoaUserControl : UserControl
    {
        private String urlPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                urlPrincipal = SPContext.Current.Site.RootWeb.Url;
                lblHTML.Text = montaLista();
            }
            catch (Exception ex)
            {
                Microsoft.Office.Server.Diagnostics.PortalLog.LogString(ex.ToString());
                lblErro.Visible = true;
                lblErro.Text = ex.ToString(); 
            }
        }

        private String retornaLinkItemLista(SPItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<li class=\"exibicaoLI\">");
            sb.Append(@"   <a href=""#"" onClick=""javascript:openDialog2(' " + urlPrincipal + "/_layouts/dbs.circulares/exibecircular.aspx?id=" + item.ID + @"',1050,'Circular')"">");            
            sb.Append("      " + item["LinkTitle"].ToString());
            sb.AppendLine("<br/>");
            sb.Append("      " + item["Descricao"].ToString());
            sb.AppendLine("   </a>");
            sb.AppendLine("</li>");


            return sb.ToString();
        }

        private String montaLista()
        {
            StringBuilder sb = new StringBuilder();
           
            SPListItemCollection lidas = getCircularesLidas();

            //Mensagens não Cientes
            SPListItemCollection naoLidas = getCircularesNaoLidas();
            if (naoLidas.Count > 0)
            {
                sb.AppendLine("<h2 class=\"exibicaoH2\"> Circulares Sem Confirmação de Ciente</h2>");
                sb.AppendLine("<br/>");
                sb.AppendLine("<ul>");

                foreach (SPItem item in naoLidas)
                {
                   sb.AppendLine(retornaLinkItemLista(item));
                }
                sb.AppendLine("</ul>");
            }

            //Mensagens já Cientes
            if (lidas.Count > 0)
            {
                sb.AppendLine("<h2 class=\"exibicaoH2\"> Circulares já Confirmadas</h2>");
                sb.AppendLine("<br/>");
                sb.AppendLine("<ul>");

                foreach (SPItem item in lidas)
                {
                    sb.AppendLine(retornaLinkItemLista(item));
                }

                sb.AppendLine("</ul>");
            }
            return sb.ToString();
        }

        private SPListItemCollection getCircularesNaoLidas()
        {
            using (SPWeb web = SPContext.Current.Site.RootWeb)
            {
                SPList lista = web.Lists["DBS.CircularesEnviadas"];
                SPQuery query = new SPQuery
                {                    
                    Query = @"       
         
                           <Where>
                              <And>
                                 <Eq>
                                    <FieldRef Name='Destinatario' LookupId='True'/>
                                    <Value Type='User'>" + web.CurrentUser.ID.ToString() + @"</Value>
                                 </Eq>
                                 <Eq>
                                    <FieldRef Name='Ciente' />
                                    <Value Type='Boolean'>True</Value>
                                 </Eq>
                              </And>
                           </Where>
                           <OrderBy>
                              <FieldRef Name='Created' Ascending='False' />
                           </OrderBy>"
                };
                return lista.GetItems(query);
            }
        }
        
        private SPListItemCollection getCircularesLidas()
        {
            using (SPWeb web = SPContext.Current.Site.RootWeb)
            {
                SPList lista = web.Lists["DBS.CircularesEnviadas"];
                SPQuery query = new SPQuery
                {
                    Query = @"        
                           <Where>
                              <And>
                                 <Eq>
                                    <FieldRef Name='Destinatario' LookupId='True'/>
                                    <Value Type='User'>" + web.CurrentUser.ID.ToString() + @"</Value>
                                 </Eq>
                                 <Neq>
                                    <FieldRef Name='Ciente' />
                                    <Value Type='Boolean'>True</Value>
                                 </Neq>
                              </And>
                           </Where>
                           <OrderBy>
                              <FieldRef Name='Created' Ascending='False' />
                           </OrderBy>"
                };
                return lista.GetItems(query);
            }
        }

    }
}
