using System.Collections.ObjectModel;
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
using l3.pkg.mapper;
using l3.Services.FileViewer;
using Microsoft.Win32;

namespace l3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private API api;
    private FileViewer fileViewer;
    private int y;
    private ObservableCollection<int> GList { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        this.api = new(new Encoder.service.Encoder(new DataProvider()));
        this.fileViewer = new FileViewer();
        this.GList = new();
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
        fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        fileDialog.Title = "Save File As";
        
        if (fileDialog.ShowDialog() == true)
        {
            this.txtOutputFile.Text = fileDialog.FileName;
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
            return;
        }

        MessageBox.Show("Complete.");
        this.showEncodedFile(this.txtOutputFile.Text);
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
            return;
        }

        MessageBox.Show("Complete.");
        
        this.showDecodedFile(this.txtOutputFile.Text);
    }
    
    public void CalculatePublicKey(object sender, EventArgs e)
    {
        var g = new List<int>();
        string msg;
        int p = int.Parse(this.txtP.Text);
        Func<string, string> errMsg = (s) => $"Failed to calculate publicKey.\n{s}";

        msg = this.validatePXK(int.Parse(this.txtP.Text), int.Parse(this.txtX.Text), int.Parse(this.txtK.Text));
        if (msg != "") {
            MessageBox.Show(errMsg(msg));
            return;
        }
        
        (g, msg) = this.api.FetchPrimitiveG(p);
        if (msg != "") {
            MessageBox.Show(errMsg(msg));
            return;
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
        this.txtInputFileContent.Text = this.fileViewer.ReadDecFile(p);
    }
    private void showEncodedFile(string p)
    {
        this.txtOutputFileContent.Text = this.fileViewer.ReadEncFile(p);
    }
    private void showDecodedFile(string p)
    {
        this.txtOutputFileContent.Text = this.fileViewer.ReadDecFile(p);
    }

    private string validatePXK(int p, int x, int k)
    {
        string msg;
        bool valid;
        (msg, valid) = Validator.P(p);
        if (!valid)
        {
            return $"Invalid input values.\nP: {msg}";
        }
        
        (msg, valid) = Validator.X(x, p);
        if (!valid)
        {
            return $"Invalid input values.\nX: {msg}";
        }
        
        (msg, valid) = Validator.K(k, p);
        if (!valid)
        {
            return $"Invalid input values.\nK: {msg}";
        }

        return "";
    }
    
}