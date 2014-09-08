<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    CodeFile="Help.aspx.vb" Inherits="Pages_Help" Title="Hitech Trader::Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="HelpText">
        <h1 class="pagetitle">
            How Our Search Works</h1>
        <ul>
            <li>Our search function uses the same set of operators (AND, +, OR, -, NOT,"") that
                most search engines use.</li>
            <li>The searches are <strong>NOT</strong> case sensitive. All letters are read as lowercase
                no matter how you type them in.</li>
        </ul>
        <h2 class="subtitle">
            Basic Search</h2>
        <br>
        <p>
            By default the system returns the results that include all the terms entered. For
            example, the search <strong><em>Beckman Model Dual</em></strong> will only give
            you the results that contain all three terms.
        </p>
        <ul>
            <li><strong><em>Beckman AND Model AND Dual</em></strong> is the same as <strong><em>
                Beckman Model Dual</em></strong>.</li>
            <li><strong><em>Beckman +Model +Dual</em></strong> is the same as <strong><em>Beckman
                AND Model AND Dual</em></strong>.</li>
            <li><strong><em>HP 5840A OR 5830A</em></strong> with give you all the results that match
                HP 5840A or HP 5830A. </li>
        </ul>
        <h2 class="subtitle">
            Exact Phrase</h2>
        <br>
        <p>
            Search terms in quotation marks <strong><em>"Dual Beam"</em></strong> are searched
            as exact phrases.
        </p>
        <ul>
            <li><strong><em>Beckman "Dual Beam"</em></strong> would search as Beckman AND Dual Beam.</li>
        </ul>
        <h2 class="subtitle">
            Exclude Certain Results</h2>
        <br>
        <p>
            You can also exclude results by using the - sign or NOT operator.
        </p>
        <ul>
            <li><strong><em>Model 507 -507E</em></strong> would give you all the results that contain
                both Model AND 507 but filter out those that have 507E. </li>
            <li><strong><em>Model 507 NOT 507E</em></strong> is the same as <strong><em>Model 507
                -507E</em></strong>.</li>
            <li>Because of the way that many model numbers are written, the system will <strong>
                NOT</strong> see <strong><em>LC-20</em></strong> the same as <strong><em>LC -20</em></strong>.
                It will treat LC-20 as a whole term and not exclude the term 20 from the results.
                You must put a space between the previous term and the - sign for it to work as
                an exclude operator.</li>
        </ul>
    </div>
</asp:Content>
