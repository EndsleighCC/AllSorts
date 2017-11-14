namespace TestWindowsService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TestWindowsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.TestWindowsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // TestWindowsServiceProcessInstaller
            // 
            this.TestWindowsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.TestWindowsServiceProcessInstaller.Password = null;
            this.TestWindowsServiceProcessInstaller.Username = null;
            // 
            // TestWindowsServiceInstaller
            // 
            this.TestWindowsServiceInstaller.ServiceName = "SimpleProcessing";
            this.TestWindowsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TestWindowsServiceProcessInstaller,
            this.TestWindowsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TestWindowsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller TestWindowsServiceInstaller;
    }
}