<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ICollection<IPST_Distributed_Web.Models.ExternalLogin>>" %>

<% if (Model.Count > 0) { %>
    <h3>Connexions externes enregistrées</h3>
    <table>
        <tbody>
        <% foreach (IPST_Distributed_Web.Models.ExternalLogin externalLogin in Model) { %>
            <tr>
                <td><%: externalLogin.ProviderDisplayName %></td>
                <td>
                    <% if (ViewBag.ShowRemoveButton) {
                        using (Html.BeginForm("Disassociate", "Account")) { %>
                            <%: Html.AntiForgeryToken() %>
                            <div>
                                <%: Html.Hidden("provider", externalLogin.Provider) %>
                                <%: Html.Hidden("providerUserId", externalLogin.ProviderUserId) %>
                                <input type="submit" value="Supprimer" title="Supprimer ces <%: externalLogin.ProviderDisplayName %> informations d’identification de votre compte" />
                            </div>
                        <% }
                    } else { %>
                        &nbsp;
                    <% } %>
                </td>
            </tr>
        <% } %>
        </tbody>
    </table>
<% } %>
