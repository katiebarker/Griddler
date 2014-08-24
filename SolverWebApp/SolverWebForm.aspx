<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolverWebForm.aspx.cs" Inherits="Griddler.Solver.SolverWebApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DropDownList ID="PuzzleDropDown" runat="server">
        </asp:DropDownList>
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="SolveButton" runat="server" Text="Solve" OnClick="SolveButton_Click" />
    
    </div>
        <asp:Table ID="aspTable" runat="server">
        </asp:Table>
    </form>
</body>
</html>
