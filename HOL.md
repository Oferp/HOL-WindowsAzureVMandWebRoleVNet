<a name="handsonlab" />
# Connecting a PaaS application to an IaaS Application with a Virtual Network#

---

<a name="Overview" />
## Overview ##

In this lab, you will create a Virtual Machine with SQL Server installed using Windows Azure Management Portal. Then you will modify and deploy a sample Web application to a new Cloud Service. By the end, you will communicate the Cloud Service and the SQL Server VM through a Virtual Network.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Directly connect a Web Role to a SQL Server running in a virtual machine through a simple virtual network
- Configure a SQL Server Virtual Machine
- Update and deploy the sample Web application to a Cloud App in Windows Azure

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio 2010](http://msdn.microsoft.com/vstudio/products/)
- [Microsoft .NET Framework 4.0](http://go.microsoft.com/fwlink/?linkid=186916)
- [Windows Azure Tools for Microsoft Visual Studio 1.7](http://www.microsoft.com/windowsazure/sdk/)
- [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4/)
- A Windows Azure subscription with the Virtual Machines Preview enabled - you can sign up for free trial [here](http://bit.ly/WindowsAzureFreeTrial)

> **Note:** This lab was designed to use Windows 7 Operating System.

---

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

- [Creating a SQL Server VM](#Exercise1)

- [Deploying a Simple MVC4 Application](#Exercise2)

 
Estimated time to complete this lab: **45 minutes**.

<a name="GettingStarted"></a>
### Getting Started - Configuring Virtual Networking ###

For this lab, you will define a virtual network where you can assign the virtual machines to specific subnets. 

<a name="Ex1Task1" />
#### Task 1 - Creating an Affinity Group ####

The first task is to create an affinity group for the Virtual Network.

1. Open a browser a go to https://manage.windowsazure.com. When prompted, login with your **Windows Azure** credentials. In the Windows Azure portal, click **Networks**, **Affinity Groups** and at the bottom click **Create**.

1. Name the affinity group _myag_, include a description and select a **Region**. Click the button to create the affinity group.

	![Creating an Affinity Group](images/creating-an-affinity-group.png?raw=true)

	_Creating an Affinity Group_

<a name="Ex1Task2" />
#### Task 2 - Creating a new Virtual Network ####

The next step is to create a new virtual network to your subscription.

1. In the Windows Azure Portal, click **Networks**, **Virtual Networks** and at the bottom click **Create**.

1. Set a Name for the virtual network, for example _MyVNET_ along with the description and click the arrow button to continue.

	![creating a new virtual network](images/creating-a-new-virtual-network.png?raw=true)

	_Creating a new virtual network_

1. Set the **Address Space** value to _192.168.0.0/16_ and add a subnet named _AppSubnet_ with a prefix of _192.168.1.0/24_.

	![Adding an address space and subnets](images/adding-an-address-space-and-subnets.png?raw=true)

	_Adding an address space and subnets_

1. Leave default settings for DNS and click the finish button.

	![Creating the Virtual Network](images/creating-the-virtual-network.png?raw=true "Creating the Virtual Network")

	_Creating the Virtual Network_

<a name="Exercise1" />
### Exercise 1: Creating a SQL Server VM ###

In this exercise, you will create a new Virtual Machine with SQL Server and configure a public endpoint in order to access it remotely.

<a name="Ex1Task1" />
#### Task 1 - Creating a VM Using Windows Azure Portal ####

In this task, you will create a new Virtual Machine using the Windows Azure Portal.

1. Navigate to the **Windows Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Windows Azure account.

1. Click **New** and select **Virtual Machine** option and then **From Gallery**.

	![Creating a New VM](images/creating-a-new-vm.png?raw=true "Creating a New VM")
 
	_Creating a New VM_

1. In the **VM OS Selection** page, click **Platform Images** on the left menu and select the **SQL Server 2012** OS image from the list. Click the arrow to continue.

1. In the **VM Configuration** page, enter a VM **Name**, complete the **Admin Password** and the **Confirmation** fields with the VM admin's password and set the VM **Size** to _Extra_ _Small_. Click the **Next** to continue.

	>**Note:** You will use these credentials in future steps to connect to the VM using remote desktop.

	![VM Configuration](images/vm-configuration.png?raw=true "VM Configuration")
 
	_Virtual Machine Configuration_

1. In the **VM Mode** page, select **Standalone Virtual Machine** option and provide a unique name for the **DNS Name**. Finally, select a **Storage Account** or leave the default value _Use Automatically Generated Storage Account_ and then select the Virtual Network you created previously from the **Region/Affinity Group/Virtual Network** list and click **Next** to contiue.

	![Selecting VM mode](images/selecting-vm-mode.png?raw=true "Selecting VM mode")

	_Setting the Virtual Machine Mode_

1. In the **VM Options** page, select the **APPSUBNET** virtual network subnet and click the finish button to create the new VM.

	![VM Options](images/vm-options.png?raw=true "VM Options")

	_Setting the Virtual Machine Options_

1. In the **Virtual Machines** section, you will see the Virtual Machine you created displaying a _provisioning_ status. Wait until it changes to _Running_ in order to continue.

	> **Note:** It will take from 8 to 10 minutes for the Virtual Machine to complete the provisioning process.

1. Now, you will create and attach empty data disks to store the SQL Server logs and data files, and you will also add an endpoint. To do this, in the **Virtual Machines** section, select the SQL Server VM you created in this task.

1. In the VM's **Dashboard**, click **Attach** in the menu at the bottom of the page and select **Attach Empty Disk**.

	![Attach Empty Disk](images/attach-empty-disk.png?raw=true "Attach Empty Disk")

	_Attach Empty Disk_

1. In the **Attach Empty Disk** page, set the **Size** to _50_ GB and create the Disk.

1. Wait until the process to attach the disk finishes. Repeat the steps 8 to 10 to create a second disk.

1. You will see three disks for the VM: one for the **OS** and other two for **Data** and **Logs**.

	> **Note:** It might take a few minutes until the data disks appear in the VM's dashboard within the Azure Portal.

<a name="Ex1Task2" />
#### Task 2 - Configuring SQL Server 2012 Instance ####

In this task, you will set up SQL Server and configure it to enable remote access.

1. In the Windows Azure Management Portal, click **Virtual Machines** on the left menu.

 	![Windows Azure Portal](./images/Windows-Azure-Portal.png?raw=true "Windows Azure Portal")
 
	_Windows Azure Portal_

1. Select your VM from the Virtual Machines list and click **Connect** to connect using **Remote Desktop Connection**.

1. In the Virtual Machine, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. Expand **Storage** node and select **Disk Management** option.

 	![Disk Management(2)](images/disk-management2.png?raw=true)
 
	_Disks Management_

1. Locate the disks you created using the **Attach Empty Disk** feature from the Windows Azure Management Portal. Right-click the first disk and select **Initialize Disk**.

1. In the **Initialize Disk** dialog, leave the default values and click **OK**.

1. Right-click the first disk unallocated space and select **New Simple Volume**.

 	![Disk Management](images/disk-management.png?raw=true)
 
	_Disks Management_

1. Follow the **New Simple Volume Wizard**. When asked for the **Volume Label** use _SQLData_.

1. Wait until the process for the first disk is completed. Repeat the steps 5 to 8 but this time using the second disk. Set the **Volume Label** to _SQLLogs_.

1. The **Disk Management** list of available disks should now show the **SQLData** and **SQLLogs** disks like in the following figure:

 	![Disks Management](./images/Disks-Management.png?raw=true "Disks Management")
 
	_Disks Management_

1. Open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.

1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLSERVER** (this option might change if you used a different instance name when installing SQL Server). Make sure **Shared Memory**, **Named Pipes** and **TCP/IP** protocols are enabled. To enable a protocol, right-click the protocol name and select **Enable**.

 	![Enabling SQL Server Protocols](./images/Enabling-SQL-Server-Protocols.png?raw=true "Enabling SQL Server Protocols")
 
	_Enabling SQL Server Protocols_

1. Go to the **SQL Server Services** node and right-click the **SQL Server (MSSQLSERVER)** item and select **Restart.**

<a name="Ex1Task3" />
#### Task 3 - Installing the AdventureWorks Database ####

In this task, you will add the **AdventureWorks** database that will be used by the sample application in the following exercise.

1. In order to enable downloads from IE you will need to update **Internet Explorer Enhanced Security Configuration**. In the Azure VM, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. In the **Server Manager** window, click **Configure IE ESC** within the **Security Information** section.

	![Configure Internet Explorer Enhanced Security](images/configure-internet-explorer-enhanced-security.png?raw=true "Configure Internet Explorer Enhanced Security")
 
	_Configure Internet Explorer Enhanced Security_

1. In the **Internet explorer Enhanced Security** dialog, turn **off** enhanced security for **Administrators** and click **OK**.

 	![Internet Explorer Enhanced Security](./images/Internet-Explorer-Enhanced-Security.png?raw=true "Internet Explorer Enhanced Security")
 
	_Internet Explorer Enhanced Security_

	>**Note:** Modifying Internet Explorer Enhanced Security configurations is not good practice and is only for the purpose of this particular lab. The correct approach should be to download the files locally and then copy them to a shared folder or directly to the VM.

1. Open the SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**.

1. Connect to the SQL Server 2012 default instance using your Windows Account.

1. Now, you will update the database's default locations in order to split the DATA from the LOGS. To do this, right click on you SQL Server instance and select **Properties**.

1. Select **Database Settings** from the left side pane.

1. Locate the **Database default locations** section and update the default values to point to the disks you attached in the previous task.

 	![Setting Database Default Locations](./images/Setting-Database-Default-Locations.png?raw=true "Setting Database Default Locations")
 
	_Setting Database Default Locations_

1. Using Windows Explorer create the following folders: **F:\Data, G:\Logs** and **G:\Backups**.

1. This lab uses the **AdventureWorks** database. Open an **Internet Explorer** browser and go to <http://msftdbprodsamples.codeplex.com/> to download  the **SQL Server 2012** sample databases. Once on the page click SQL Server 2012 DW and then download Adventure Works 2012 Data File. Download the file to F:\Data.

1. Right click the database file and open the properties. Click **unblock**.

1. Add the **AdventureWorks** sample database to your SQL Server. To do this, open **SQL Server Management Studio**, connect to **(local)** using your Windows Account. Locate your SQL Server instance node and expand it.

1. Right click **Databases** folder and select **Attach**.

	![Object Explorer - Attaching AdventureWorks Database](images/attaching-adventureworks-database-menu.png?raw=true)
 
	_Object Explorer - Attaching Adventureworks Database_

1. In the **Attach Databases** dialog, press **Add**. Browse to the data disk and select the Adventure Works data file.

1. Now, select the AdventureWorks Log's row within **database details** and click **Remove**.

 	![Attaching AdventureWorks Database](./images/attaching-adventureworks-database.png?raw=true "Attaching AdventureWorks Database")
 
	_Attaching AdventureWorks Database_

1. Click **OK** to attach the database. 

1. Create a Full Text Catalog for the database. You will consume this feature with a MVC application you will deploy in the next exercise. To do this, expand **Storage** node within **AdventureWorks** database.

1. Right-click **Full Text Catalogs** folder and select **New Full-Text Catalog**.

	![New Full-Text Catalog](images/new-full-text-catalog.png?raw=true "New Full-Text Catalog")
 
	_New Full-Text Catalog_

1. In the **New Full-Text Catalog** dialog, set the **Name** value to _AdventureWorksCatalog_ and press **OK**.

	![New Full-Text Catalog Name](images/new-full-text-catalog-name.png?raw=true "New Full-Text Catalog Name")
 
	_Full-Text Catalog Name_

1. Right-click the **AdventureWorksCatalog** and select **Properties**. Select the **Tables/Views** menu item. Add the **Production.Product** table to the **Table/View objects assigned to the Catalog** list. Check _Name_ from **eligible columns** and click **OK**.

	![Full-Text Catalog Properties](images/full-text-catalog-properties.png?raw=true "Full-Text Catalog Properties")
 
	_Full-Text Catalog Properties_

1. Enable **Mixed Mode Authentication** to the SQL Server instance. To do this, in the **SQL Server Management Studio**, right-click the server instance and click **Properties**.

1. Select the **Security** page in the right side pane and then select **SQL Server and Windows Authentication mode** under **Server Authentication** section. Click **OK** to save changes.

    ![Mixed authentication mode](images/mixed-authentication-mode.png?raw=true "Mixed authentication mode")

    _Mixed authentication mode_

1. Restart the SQL Server instance. To do this, right-click the SQL Server instance and click **Restart**.

1. Add a new user for the MVC4 application you will deploy in the following exercise. To do this, expand **Security** folder within the SQL Server instance. Right-click **Logins** folder and select **New Login**.

 	![Creating a New Login](./images/create-new-login.png?raw=true "Creating a New Login")
 
	_Creating a New Login_

1. In the **General** section, set the **Login name** to _AzureStore._ Select **SQL Server authentication** option and set the **Password** to _Azure$123_.

	>**Note:** If you entered a different username or password than those suggested in this step you will need to update the web.config file for the MVC 4 application you will use in the next exercise to match those values

1. Uncheck **Enforce password policy** option to avoid having to change the User's password the first time you log on and set the **Default database** to _AdventureWorks_.

	![New Login's General Settings](images/new-logins-general-settings.png?raw=true "New Login's General Settings")
 
	_Creating a New Login_

1. Go to **User Mapping** section. Map the user to the _AdventureWorks_ database and click **OK**.

 	![Mapping the new User to the AdventureWorks Database](./images/Mapping-the-new-User.png?raw=true "Mapping the new User to the AdventureWorks Database")
 
	_Mapping the new User to the AdventureWorks Database_

1. Expand **AdventureWorks** database within **Databases** folder. In the **Security** folder, expand **Users** and double-click **AzureStore** user.

1. Select the **Membership** page, and select the _db_owner_ role checkbox for the **AzureStore** user and click **OK**.

 	![Adding Database role membership to AzureStore user](./images/Adding-Database-role-membership-to-AzureStore-user.png?raw=true "Adding Database role membership to AzureStore user")
 
	_Adding Database role membership to AzureStore user_

	>**Note:** The application you will deploy in the next exercise uses Universal Providers to manage sessions. The first time the application run the Provider will create a Sessions table within AdventureWorks database. For that reason, you are assigning db_owner role for the AzureStore user. Once you run the application for the first time, you can remove this role for the user as it will not need those permissions anymore.

1. Close the **SQL Server Management Studio**.

1. In order to allow the MVC4 application access the SQL Server database you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advance Security** from **Start | All Programs | Administrative Tools**.

1. Select **Inbound Rules** node, right-click it and select **New Rule**.

 	![Creating an Inbound Rule](./images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")
 
	_Creating an Inbound Rule_

1. In the **New Inbound Rule Wizard**, select _Port_ as **Rule Type** and click **Next**.

	![New Inbound Rule Type](images/new-inbound-rule-type.png?raw=true "Inbound Rule Type")
 
	_Inbound Rule's Type_

1. In **Protocols and Ports** step, select **Specific local ports** and set its value to _1433_. Click **Next** to continue.

	![Inbound Rule's Local Port](images/inbound-rules-local-port.png?raw=true "Inbound Rule's Local Port")
 
	_Inbound Rule's Local Port_

1. In the **Action** step, make sure **Allow the connection** option is selected and click **Next**.

	![Inbound Rule's Action](images/inbound-rules-action.png?raw=true "Inbound Rule's Action")
 
	_Inbound Rule's Action_

1. In the **Profile** step, leave the default values and click **Next**.

1. Finally, set the Inbound Rule's **Name** to _SQLServerRule_ and click **Finish**.

 	![New Inbound Rule](images/new-inbound-rule.png?raw=true "New Inbound Rule")
 
	_New Inbound Rule_

1. Close **Windows Firewall with Advanced Security** window and then close the **Remote Desktop Connection**.


<a name="Exercise2" />
### Exercise 2: Deploying a Simple MVC4 Application ###

In this exercise, you will configure a simple Web application to connect to the SQL Server instance you created in the previous exercise and publish the application to **Windows Azure** and run it in the Cloud.

<a name="Ex2Task1" />
#### Task 1 - Configuring the MVC4 Application to Connect to an SQL Server Instance ####

In this task, you will change the connection string to point to the SQL Server instance created in the previous exercise and update the configuration settings to enable network communication between the Web Role and the SQL Server instance.

1. Navigate to the **Windows Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Windows Azure account.

1. In the left side pane, click on **Virtual Machines** and locate the SQL Server Virtual Machine you created in the previous exercise. Select the VM and then click **Connect**. 

1. When prompted to save or open the .rdp file, click **Open** and then log on using the Admin credentials you defined when you created the VM.

1. Once logged on, open a **Command Prompt** window and type **ipconfig** to display the machine's IP configuration.

1. Take note of the machine's ip address as you will use it later.

	![Getting the IP address](images/getting-the-ip-address.png?raw=true "Getting the IP address")

	_Getting the IP address_

1. Open Visual Studio as administrator from **Start | All Programs | Microsoft Visual Studio 2010** by right clicking the Microsoft Visual Studio 2010 shortcut and choosing **Run as administrator**.

1. Open the solution **IaaSDeploySimpleApp.sln** located in the folder **Ex02-DeploySampleApp** under the **Source** folder of this lab.

1. Compile the solution in order to download the required packages.

1. Open the **Web.config** file and locate the **connectionStrings** node at the end of the file. Replace the **Data Source** attribute values with the IP address of the  SQL Server Virtual Machine you copied in step 2.

	<!--mark: 1-5-->
	````XML
	<connectionStrings>
		<add name="DefaultConnection" connectionString="Data Source=[ENTER-IP-ADDRESS];initial catalog=AdventureWorksLT2008R2;Uid=AzureStore;Password=Azure$123;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
		<add name="AdventureWorksEntities" connectionString="metadata=res://*/Models.AdventureWorks.csdl|res://*/Models.AdventureWorks.ssdl|res://*/Models.AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=[ENTER-IP-ADDRESS];initial catalog=AdventureWorksLT2008R2;Uid=AzureStore;Password=Azure$123;multipleactiveresultsets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
	</connectionStrings>
	````

1. In order to enable communication between the Web Role and the SQL Server VM, you need configure the Web Role to connect to the same **Virtual Network** as the SQL Server VM. To do so, open the **ServiceConfiguration.Cloud.cscfg** under the **AzureStore.Azure** project and add the highlighted  configuration:

	<!--mark: 9-18-->
	````XML
<?xml version="1.0" encoding="utf-8"?>
<ServiceConfiguration serviceName="AzureStore.Azure" xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration" osFamily="1" osVersion="*" schemaVersion="2012-05.1.7">
  <Role name="AzureStore">
    <Instances count="1" />
    <ConfigurationSettings>
      <Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value="UseDevelopmentStorage=true" />
    </ConfigurationSettings>
  </Role>
  <NetworkConfiguration>
    <VirtualNetworkSite name="MyVNET" />
    <AddressAssignments>
      <InstanceAddress roleName="AzureStore">
        <Subnets>
          <Subnet name="AppSubnet" />
        </Subnets>
      </InstanceAddress>
    </AddressAssignments>
  </NetworkConfiguration>
</ServiceConfiguration>
````

<a name="Ex2Task2" />
#### Task 2 - Publishing the MVC4 Application to Windows Azure ####

In this task, you will publish the Web Application to Windows Azure using Visual Studio.

1. In Visual Studio, right-click the **AzureStore.Azure** project and select **Package**.

	![Packaging the Cloud Application](images/packaging-the-cloud-application.png?raw=true "Packaging the Cloud Application")

	_Packaging the Cloud Application_

1. In the **Package Windows Azure Application** dialog, make sure that _Service Configuration_ is set to **Cloud** and _Build Configuration_ is set to **Release**. Then click the **Package** button.

	![Package Windows Azure Application dialog](images/package-windows-azure-application-dialog.png?raw=true "Package Windows Azure Application dialog")

	_Package Windows Azure Application dialog_

1. Wait to the package process to finish to continue with the next step.

1. Navigate to the **Windows Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Windows Azure account.

1. Click the **New** link located at the bottom of the page, select **Cloud Service** and then **Custom Create**.

1. In the **Create Your Cloud Service** window, enter **AzureStore** in the **Url** field, select **myag** from the **Region/Affinity Group** selection list and check the **Deploy a Cloud Service package now** option.

	![New Cloud Service](images/new-cloud-service5.png?raw=true "New Cloud Service")

	_New Cloud Service_

1. In the **Publish you Cloud Service** window, enter a name for the new deployment (for instance **AzureStore**). Enter the location for your package and configuration files (ussually under the bin\Release\app.publish folder of your cloud project) and check the **Deploy even if one or more roles contain a single instance** option. Then click the **Finish** button.

	![New Cloud Service](images/new-cloud-service4.png?raw=true "New Cloud Service")

1. Wait until your new _Cloud Service_ is deployed and provisioned.


1. Once the _Cloud Service_ status gets to **Ready** click on the service's name to go to the **Dashboard** page. Once there, click the **Site Url** link in the **Quick Glance** pane.

	![Cloud Service Dashboard](images/cloud-service-dashboard.png?raw=true "Cloud Service Dashboard")

	_Cloud Service Dashboard_

1. The browser will show you the home page of the **Azure Store** sample application. In the **Search** box, write _Classic_ and click **Search**. It will show all the products that match the search criteria. The Cloud App is accessing the SQL Server using the public endpoint to retrieve the list of products.

 	![Searching Products](./images/Searching-Products.png?raw=true "Searching Products")
 
	_Searching Products_

---

