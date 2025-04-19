using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using l3.Encoder.data;
using l3.Encoder.transport;
using Microsoft.Win32;

namespace l3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private API api;
    private int y;
    public MainWindow()
    {
        InitializeComponent();
        this.api = new(new Encoder.service.Encoder(new DataProvider()));
    }

    public void FetchFileToLoad(object sender, EventArgs e)
    {
        var fileDialog = new OpenFileDialog();
        if (fileDialog.ShowDialog() == true)
        {
            this.txtInputFile.Text = fileDialog.FileName;
            this.showInputFile(fileDialog.FileName);
        }
    }
    public void FetchFileToSave(object sender, EventArgs e)
    {
        var fileDialog = new SaveFileDialog();
        fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        fileDialog.Title = "Save File As";
        
        if (fileDialog.ShowDialog() == true)
        {
            this.txtOutputFile.Text = fileDialog.FileName;
            this.showInputFile(fileDialog.FileName);
        }
    }

    public void Encode(object sender, EventArgs e)
    {
        string msg = this.api.Encode(
            int.Parse(this.txtP.Text), 
            (int)this.gList.SelectedValue,
            this.y,
            int.Parse(this.txtK.Text),
            this.txtOutputFile.Text,
            this.txtInputFile.Text
        );
        if (msg != "")
        {
            MessageBox.Show(msg);
        }
    }
    public void Decode(object sender, EventArgs e)
    {
        string msg = this.api.Decode(
            int.Parse(this.txtP.Text),
            int.Parse(this.txtX.Text),
            this.txtOutputFile.Text,
            this.txtInputFile.Text
        );
        if (msg != "")
        {
            MessageBox.Show(msg);
        }
    }
    
    public void CalculatePublicKey(object sender, EventArgs e)
    {
        List<int> g = new List<int>();
        string msg;
        int p = int.Parse(this.txtP.Text);
        
        Func<string, string> errMsg = (s) => $"Failed to calculate publicKey.\n{s}";
        (g, msg) = this.api.FetchPrimitiveG(p);
        if (msg != "") {
            MessageBox.Show(errMsg(msg));
        }

        gList.ItemsSource = g;
        gList.SelectedIndex = 0;

        this.createNewPublicKey();

    }

    private void assemblePublicKey(string p, string g, string y)
    {
        this.txtPulicKey.Text = $"{{\"P\":{p},\"G\":{g},\"Y\":{y})";
    }

    private void SetNewG(object sender, object e)
    {
        this.createNewPublicKey();
    }

    private void createNewPublicKey()
    {
        this.y = this.api.CalcY((int)this.gList.SelectedValue,int.Parse(this.txtX.Text), int.Parse(this.txtP.Text));
        this.assemblePublicKey(this.txtP.Text, this.gList.SelectedValue.ToString(), this.y.ToString());
    }

    private void showInputFile(string p)
    {
        var bytes = File.ReadAllBytes(p);
        StringBuilder s = new StringBuilder(bytes.Length*4);
        foreach (var b in bytes)
        {
            s.Append(b.ToString("D3"));
            s.Append(",");
        }
        this.txtInputFileContent.Text = s.ToString();
    }
}