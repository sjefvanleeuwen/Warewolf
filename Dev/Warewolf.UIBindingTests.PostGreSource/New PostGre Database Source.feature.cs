﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.UIBindingTests.PostGreSource
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class NewPostgreSqlSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "New PostGre Database Source.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "New PostgreSql Source", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
                    "f two numbers\t", ProgrammingLanguage.CSharp, new string[] {
                        "DbSource"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "New PostgreSql Source")))
            {
                global::Warewolf.UIBindingTests.PostGreSource.NewPostgreSqlSourceFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(TestContext);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source General Testing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void CreatingNewDBSourceGeneralTesting()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source General Testing", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 31
this.ScenarioSetup(scenarioInfo);
#line 32
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 33
   testRunner.Then("\"New PostgreSQL Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
   testRunner.And("title is \"New PostgreSQL Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
   testRunner.When("I type Server as \"RSAKLFSVR\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Options"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRDEV"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRPDC"});
            table1.AddRow(new string[] {
                        "TFSBLD.dev2.local"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRWRWBLD"});
#line 36
   testRunner.Then("the intellisense contains these options", ((string)(null)), table1, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Options"});
            table2.AddRow(new string[] {
                        "Microsoft SQL Server"});
            table2.AddRow(new string[] {
                        "MySQL"});
#line 42
   testRunner.And("type options contains", ((string)(null)), table2, "And ");
#line 46
   testRunner.And("I type Select The Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
   testRunner.And("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
   testRunner.Then("Username field is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 52
   testRunner.And("Password field is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
   testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 54
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
   testRunner.And("\"Cancel Test\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
   testRunner.When("I click \"Test Connection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
   testRunner.Then("\"Cancel Test\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
   testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
   testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 60
   testRunner.Then("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
   testRunner.When("I save the source as \"SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
   testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
   testRunner.Then("\"SavedDBSource\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 65
   testRunner.And("title is \"SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
   testRunner.When("I type Server as \"RSA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 67
   testRunner.Then("\"SavedDBSource *\" is the tab Header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 68
   testRunner.And("title is \"SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source as User Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void CreatingNewDBSourceAsUserAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source as User Auth", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 76
this.ScenarioSetup(scenarioInfo);
#line 77
    testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 78
    testRunner.And("I type Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
    testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
    testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 81
    testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
    testRunner.When("I type Username as \"postgres\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
    testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 86
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
    testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
    testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 90
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
    testRunner.When("I select \"postgres\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 93
    testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 94
    testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 95
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server Address Doesnt Allow Save Windows Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void IncorrectServerAddressDoesntAllowSaveWindowsAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server Address Doesnt Allow Save Windows Auth", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 103
this.ScenarioSetup(scenarioInfo);
#line 104
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 105
      testRunner.And("I type Server as \"TFSBLD.dev2.local\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
      testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
      testRunner.Then("Username field is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 109
      testRunner.And("Password field is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 111
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 113
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 114
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server Address Doesnt Allow Save User Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void IncorrectServerAddressDoesntAllowSaveUserAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server Address Doesnt Allow Save User Auth", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 122
this.ScenarioSetup(scenarioInfo);
#line 123
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 124
      testRunner.And("I type Server as \"TFSBLD.dev2.local\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 127
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
   testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 129
   testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 131
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 133
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 134
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server Address Shows correct error message")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void IncorrectServerAddressShowsCorrectErrorMessage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server Address Shows correct error message", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 142
this.ScenarioSetup(scenarioInfo);
#line 143
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 144
      testRunner.And("I type Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 147
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
   testRunner.When("I type Username as \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 149
   testRunner.And("I type Password as \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 151
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 152
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 153
      testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 154
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
   testRunner.And("the error message is \"Login failed for user \'test\'\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved DB Source Remembers Previous Auth Selection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void EditingSavedDBSourceRemembersPreviousAuthSelection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved DB Source Remembers Previous Auth Selection", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 164
this.ScenarioSetup(scenarioInfo);
#line 165
 testRunner.Given("I open \"Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 166
    testRunner.And("Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
    testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
    testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
    testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 172
 testRunner.When("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 173
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 174
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 175
    testRunner.And("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
    testRunner.And("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 179
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
    testRunner.And("I select \"Dev2TestingDB2\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
    testRunner.When("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 183
    testRunner.Then("\"Test Connection\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 184
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 186
    testRunner.And("\"Test Connection\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 187
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved DB Source Remembers credentials")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void EditingSavedDBSourceRemembersCredentials()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved DB Source Remembers credentials", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 195
this.ScenarioSetup(scenarioInfo);
#line 196
 testRunner.Given("I open \"Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 197
    testRunner.And("Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
    testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
    testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
    testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 202
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 203
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 204
 testRunner.When("I Edit Server as \"RSAKLFSVRWRWBLD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 205
 testRunner.Then("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 206
    testRunner.Then("Username is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 207
    testRunner.And("Password  is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 208
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 209
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 210
    testRunner.Then("Database dropdown is \"Collapsed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 211
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 212
    testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 213
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Cancel DB Source Test")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void CancelDBSourceTest()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel DB Source Test", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 221
this.ScenarioSetup(scenarioInfo);
#line 222
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 223
   testRunner.When("I type Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 224
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 225
   testRunner.And("\"Test Connection\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 226
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 227
   testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 228
   testRunner.And("I type Password as \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 229
   testRunner.When("Test Connecton is \"Long Running\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 230
   testRunner.And("I Cancel the Test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 231
   testRunner.Then("the validation message as \"Test Cancelled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 232
   testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 233
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Changing database type after testing connection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New PostgreSql Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DbSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostgresDbSource")]
        public virtual void ChangingDatabaseTypeAfterTestingConnection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Changing database type after testing connection", new string[] {
                        "PostgresDbSource",
                        "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                            "1.dll",
                        "MSTest:DeploymentItem:Warewolf_Studio.exe",
                        "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                        "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                        "MSTest:DeploymentItem:System.Windows.Interactivity.dll"});
#line 242
this.ScenarioSetup(scenarioInfo);
#line 243
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 244
   testRunner.Then("\"New PostgreSQL Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 245
   testRunner.And("title is \"New PostgreSQL Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 246
   testRunner.When("I type Server as \"RSAKLFSVR\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Options"});
            table3.AddRow(new string[] {
                        "RSAKLFSVRDEV"});
            table3.AddRow(new string[] {
                        "RSAKLFSVRPDC"});
            table3.AddRow(new string[] {
                        "TFSBLD.dev2.local"});
            table3.AddRow(new string[] {
                        "RSAKLFSVRWRWBLD"});
#line 247
   testRunner.Then("the intellisense contains these options", ((string)(null)), table3, "Then ");
#line 253
   testRunner.And("I type Select The Server as \"RSAKLFSVRDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 254
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 255
    testRunner.When("I type Username as \"postgres\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 256
    testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 257
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 258
   testRunner.When("I click \"Test Connection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 259
   testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 260
   testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 261
   testRunner.Then("I select \"postgres\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 262
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 263
   testRunner.And("I type Select The Server as \"RSAKLFSVRPDC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 264
   testRunner.Then("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 265
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
