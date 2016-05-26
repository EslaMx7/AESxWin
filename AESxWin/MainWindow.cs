using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AESxWin.Helpers;
using System.Threading;

namespace AESxWin
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SetLogViewer(txtLog);
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            lstExts.SelectedIndex = 6;

        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Select your File(s)";
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                fileDialog.Multiselect = true;
                fileDialog.SupportMultiDottedExtensions = true;
                fileDialog.InitialDirectory = Application.StartupPath;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    var files = fileDialog.FileNames;

                    if (files != null && files.Length > 0)
                    {
                        foreach (var filePath in files)
                        {
                            var items = lstPaths.Items;
                            if (!items.Contains(filePath))
                                lstPaths.Items.Add(filePath);
                            else
                                this.Log(filePath + " is already exist in the list.");
                        }
                    }
                }
            }
        }

        private void btnRemovePath_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstPaths.SelectedIndex;
            if (selectedIndex != -1)
            {
                lstPaths.Items.RemoveAt(selectedIndex);

                lstPaths.SelectedIndex = selectedIndex < lstPaths.Items.Count ? selectedIndex : selectedIndex - 1;
                lstPaths.Focus();
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select A Folder";
                folderDialog.ShowNewFolderButton = true;
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    var folderPath = folderDialog.SelectedPath;
                    if (!String.IsNullOrEmpty(folderPath))
                    {
                        var items = lstPaths.Items;
                        if (!items.Contains(folderPath))
                            lstPaths.Items.Add(folderPath);
                        else
                            this.Log(folderPath + " is already exist in the list.");
                    }
                }


            }
        }

        private async void btnEncrypt_Click(object sender, EventArgs e)
        {
            var count = 0;
            var paths = lstPaths.Items;
            
            this.Log("Encryption Started.");

            if (paths != null && paths.Count > 0)
            {
                foreach (string path in paths)
                {

                    if (File.Exists(path)) // Is File 
                    {
                        if (path.CheckExtension(lstExts.Text.ParseExtensions()))
                        {
                            try
                            {
                                await path.EncryptFileAsync(txtPassword.Text);
                                this.Log(path + " Encrypted.");
                                count++;

                                if (chkDeleteOrg.Checked)
                                    File.Delete(path);
                            }
                            catch (Exception ex)
                            {

                                this.Log(path + " " + ex.Message);
                            }

                        }
                    }
                    if (Directory.Exists(path)) // Is Folder
                    {
                        var followSubDirs = chkSubFolders.Checked ? true : false;

                        var allfiles = path.GetFolderFilesPaths(followSubDirs);

                        foreach (var file in allfiles)
                        {
                            if (file.CheckExtension(lstExts.Text.ParseExtensions()))
                            {
                                if (!file.EndsWith(".aes"))
                                {
                                    try
                                    {
                                        await file.EncryptFileAsync(txtPassword.Text);
                                        this.Log(file + " Encrypted.");
                                        count++;

                                        if (chkDeleteOrg.Checked)
                                            File.Delete(file);
                                    }
                                    catch (Exception ex)
                                    {

                                        this.Log(file + " " + ex.Message);
                                    }
                                }
                                else
                                {
                                  //  this.Log(file + " Ignored.");
                                }
                            }
                        }


                    }


                }
            }



            this.Log($"Finished : {count} File(s) Encrypted.");


        }

        private async void btnDecrypt_Click(object sender, EventArgs e)
        {
            var count = 0;
            var paths = lstPaths.Items;

            this.Log("Decryption Started.");

            if (paths.Count > 0)
            {
                foreach (string path in paths)
                {

                    if (File.Exists(path) && path.EndsWith(".aes")) // Is Encrypted File 
                    {
                        try
                        {
                            await path.DecryptFileAsync(txtPassword.Text);
                            this.Log(path + " Decrypted.");
                            count++;

                            if (chkDeleteOrg.Checked)
                                File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            this.Log(path + " " + ex.Message);
                            if (File.Exists(path.RemoveExtension()))
                                File.Delete(path.RemoveExtension());
                        }


                    }
                    if (Directory.Exists(path)) // Is Folder
                    {
                        var followSubDirs = chkSubFolders.Checked ? true : false;

                        var allfiles = path.GetFolderFilesPaths(followSubDirs);

                        foreach (var file in allfiles)
                        {
                            if (file.RemoveExtension().CheckExtension(lstExts.Text.ParseExtensions()))
                            {
                                if (file.EndsWith(".aes"))
                                {
                                    try
                                    {
                                        await file.DecryptFileAsync(txtPassword.Text);
                                        this.Log(file + " Decrypted.");
                                        count++;

                                        if (chkDeleteOrg.Checked)
                                            File.Delete(file);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.Log(file + " " + ex.Message);
                                        if(File.Exists(file.RemoveExtension()))
                                            File.Delete(file.RemoveExtension());
                                    }
                                }

                            }
                            else
                            {
                               // this.Log(file + " Ignored.");
                            }
                        }


                    }


                }
            }

            this.Log($"Finished : {count} File(s) Decrypted.");
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            Process.Start("http://eslamx.com");
        }
    }
}
