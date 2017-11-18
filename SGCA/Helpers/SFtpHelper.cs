using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using Renci.SshNet;

namespace SGCA.Helpers
{
    public static class SFtpHelper
    {
        private static ConfigSFtp config;
        private static IApplicationManager appManager;
        private static string LOCAL_ARQUIVOS_TEMPORARIOS = "~/tempData/";

        /// <summary>
        ///     Upload File To Specific folder
        /// </summary>
        public static void UploadFtpFile(ConnectionInfo connectionInfo, HttpPostedFileBase requestFile,string folder, string id)
        {            
            try
            {
                string fileName = null;

                if (id == null)
                {
                    //Get File Name + ID - Key to find the file on SFTP server
                    fileName = Path.GetFileName(requestFile.FileName);
                }
                else
                {
                    //Get File Name + ID - Key to find the file on SFTP server
                    fileName = id + "_" + Path.GetFileName(requestFile.FileName);
                }
                //Create the file on Temp Data
                AddFileOnTempData(id, requestFile);
                //Get sftp object
                using (var sftp = new SftpClient(connectionInfo))
                {
                    //Open Connection
                    sftp.Connect();
                    //Open File on Temp Data
                    using (var file = System.IO.File.Open(HttpContext.Current.Server.MapPath(LOCAL_ARQUIVOS_TEMPORARIOS) + fileName, 
                                         FileMode.Open, FileAccess.Read))
                    {
                        //Upload File on SFTP Server
                        sftp.UploadFile(file, ConfigSftp.Dsc_path + folder + fileName);
                    }
                    //Close Connection
                    sftp.Disconnect();
                }
                //Delete file from TEMP DATA
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(LOCAL_ARQUIVOS_TEMPORARIOS) + fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Upload File To Specific folder
        /// </summary>
        public static void UploadFtpFile(ConnectionInfo connectionInfo, string name, string folder, string id)
        {
            try
            {
                //Get File Name + ID - Key to find the file on SFTP server
                string fileName = id + "_" + name;
                //Get sftp object
                using (var sftp = new SftpClient(connectionInfo))
                {
                    //Open Connection
                    sftp.Connect();
                    //Open File on Temp Data
                    using (var file = System.IO.File.Open(HttpContext.Current.Server.MapPath(LOCAL_ARQUIVOS_TEMPORARIOS) + fileName,
                                         FileMode.Open, FileAccess.Read))
                    {
                        //Upload File on SFTP Server
                        sftp.UploadFile(file, ConfigSftp.Dsc_path + folder + fileName);
                    }
                    //Close Connection
                    sftp.Disconnect();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Download File from a Specific Folder
        /// </summary>
        public static FileStream DownloadFtpFile(ConnectionInfo connectionInfo,string folder ,string fileName, string id)
        {
            try
            {
                //Concat File with Id User Session
                string idFileName = ((Usuario)HttpContext.Current.Session["usuario"]).Id_usuario + "_" + fileName;
                //Create a file Stream object
                FileStream file = null;
                //Get Sftp Connection
                using (var sftp = new SftpClient(connectionInfo))
                {
                    //Open Connection
                    sftp.Connect();
                    //Create a file on Temporary Folder
                    file = System.IO.File.Open(HttpContext.Current.Server.MapPath(LOCAL_ARQUIVOS_TEMPORARIOS) + idFileName, FileMode.Create, FileAccess.Write);
                    //Download File
                    if (id != null)
                    {
                        sftp.DownloadFile(ConfigSftp.Dsc_path + folder + id + "_" + fileName, file);
                    }
                    else
                    {
                        sftp.DownloadFile(ConfigSftp.Dsc_path + folder + fileName, file);
                    }
                    //Close File
                    file.Close();
                    //Close Connection
                    sftp.Disconnect();
                }
                //Return the file stream
                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Delete File from a Specific Folder
        /// </summary>
        public static void DeleteFtpFile(ConnectionInfo connectionInfo, string folder, string fileName, string id)
        {
            try
            {
                //Get Sftp Connection
                using (var sftp = new SftpClient(connectionInfo))
                {
                    //Open Connection
                    sftp.Connect();
                    //Create a file on Temporary Folder
                    if (id != null)
                    {
                        sftp.Delete(ConfigSftp.Dsc_path + folder + id + "_" + fileName);
                    }
                    else
                    {
                        sftp.Delete(ConfigSftp.Dsc_path + folder + fileName);
                    }
                    //Close Connection
                    sftp.Disconnect();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Checks if the file Exists on the ftp file path
        /// </summary>
        /// <returns></returns>
        public static bool ExistsFileInFtpPath(ConnectionInfo connectionInfo, string folder, string fileName, string id)
        {
            bool existsFile = false;

            using (var sftp = new SftpClient(connectionInfo))
            {
                //Open Connection
                sftp.Connect();
                if (id != null)
                {
                    //Checks if File exists in sftp path
                    existsFile = sftp.Exists(ConfigSftp.Dsc_path + folder + id + "_" + fileName);
                }
                else
                {
                    //Checks if File exists in sftp path
                    existsFile = sftp.Exists(ConfigSftp.Dsc_path + folder + fileName);
                }
                //Close Connection
                sftp.Disconnect();
            }
            //Return True or False
            return existsFile;
        }

        /// <summary>
        ///     Return Basic Connection Info
        /// </summary>
        /// <returns></returns>
        public static ConnectionInfo GetConnectionInfo()
        {
            return new PasswordConnectionInfo(ConfigSftp.Dsc_host, ConfigSftp.Num_port, ConfigSftp.Dsc_username, ConfigSftp.Dsc_password);
        }

        /// <summary>
        ///     Return a Custom Connection Info
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ConnectionInfo GetConnectionInfo(string host, int port, string user, string password)
        {
            return new PasswordConnectionInfo(host, port, user, password);
        }

        /// <summary>
        /// Singleton ConfigSftp
        /// </summary>
        private static ConfigSFtp ConfigSftp 
        {
            get 
            {
                if (config == null)
                {
                    ConfigSftp = AppManager.FindConfigSFtp();
                }
                return config; 
            }
            set 
            { 
                config = value; 
            }
        }

        /// <summary>
        ///     Application Manager
        /// </summary>
        internal static IApplicationManager AppManager
        {
            get { return ServiceLocator.GetObject<IApplicationManager>(); }
            set { appManager = value; }
        }

        /// <summary>
        ///     Add file on a temporary folder
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="arquivoExcelNoRequest">Arquivo anexado no request</param>
        private static void AddFileOnTempData(string id, HttpPostedFileBase requestFile)
        {
            string fileName = null;

            if (id == null)
            {
                fileName = Path.GetFileName(requestFile.FileName);
            }
            else
            {
                fileName = id + "_" + Path.GetFileName(requestFile.FileName);
            }
            var path = Path.Combine(HttpContext.Current.Server.MapPath(LOCAL_ARQUIVOS_TEMPORARIOS), fileName);
            requestFile.SaveAs(path);
        }
    }
}