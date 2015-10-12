using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Framework.UI.Controls;

namespace RealEstate.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for AgentView.xaml
    /// </summary>
    public partial class AgentView 
    {
        public AgentView()
        {
            InitializeComponent();
        }

        private void RE_Agents_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshAgents();
        }

        private void BT_AddAgent_Click(object sender, RoutedEventArgs e)
        {
            OpenAddAgentOverlay();
        }

        private void BT_EditAgent_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Agents.SelectedIndex != -1)
            {
                OpenEditAgentOverlay();
            }
            else
            {
                DisplayNotifyBox("Cannot edit", "Please select an agent to edit", 2);
            }
        }

        private void DisplayNotifyBox(string title, string message, int duration)
        {
            this.Dispatcher.Invoke(() =>
            {
                NotifyBox.Show(null, title, message, new TimeSpan(0, 0, duration), false);
            });
        }

        private async void BT_DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            if( DG_Agents.SelectedIndex != -1 )
            {
                MessageBoxResult result = await MessageDialog.ShowAsync("Are you Sure?", "Are you sure you want to delete " + GetSelectedEmail(), MessageBoxButton.YesNo, MessageDialogType.Light);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteAgent();
                }
                RefreshAgents();
            }
            else
            {
                DisplayNotifyBox("Cannot delete", "Please select an agent to delete", 2);
            }
        }

        private void DeleteAgent()
        {
            new System.Threading.Thread(() =>
            {
                Classes.AgentManager agentManager = new Classes.AgentManager();

                if (agentManager.DeleteAgent(GetSelectedEmail()))
                {
                    DisplayNotifyBox("Deleted", "The record has been deleted", 3);
                }
                else
                {
                    DisplayNotifyBox("Not Deleted", GetSelectedName() + " " + GetSelectedSurname() + " has not been deleted", 3);
                }

            }).Start();            
        }

        private void AgentOverlays_OnClose(object sender, EventArgs e)
        {
            RefreshAgents();
        }

        private void OpenAddAgentOverlay()
        {
            Overlays.Agent.AddAgentOverlay agentOverlay = new Overlays.Agent.AddAgentOverlay();
            agentOverlay.OnExit += AgentOverlays_OnClose;
            agentOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            agentOverlay.Show();
        }

        private void OpenEditAgentOverlay()
        {
            Overlays.Agent.EditAgentOverlay agentOverlay = new Overlays.Agent.EditAgentOverlay(GetSelectedEmail());
            agentOverlay.OnExit += AgentOverlays_OnClose;
            agentOverlay.Owner = Framework.UI.Controls.Window.GetWindow(this);
            agentOverlay.Show();
        }

        private void BT_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshAgents();
        }

        private void RefreshAgents()
        {
            new System.Threading.Thread(() =>
            {
                ClearAgentsGrid();
                Classes.DatabaseManager dbManager = new Classes.DatabaseManager();
                var agentNames = dbManager.ReturnQuery("SELECT Agent_Name, Agent_Surname, Agent_Phone, Agent_Email FROM Agent ORDER BY Agent_Surname, Agent_Name;");

                foreach (var agent in agentNames)
                {
                    InsertIntoAgentsGrid(agent[0], agent[1], agent[2], agent[3]);
                }

            }).Start();
        }

        private void ClearAgentsGrid()
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Agents.Items.Clear();
            });
        }

        private void InsertIntoAgentsGrid(string agentName, string agentSurname, string agentPhone, string agentEmail)
        {
            this.Dispatcher.Invoke(() =>
            {
                DG_Agents.Items.Add(new GridViewSources.Agent() { Name = agentName, Surname = agentSurname, Phone = agentPhone, Email = agentEmail });
            });
        }

        private void DG_Agents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DG_Agents.SelectedIndex != -1)
            {
                TB_SelectedAgent.Text = GetSelectedName() + " " + GetSelectedSurname();
            }
            else
            {
                TB_SelectedAgent.Text = "";
            }
            
        }

        private string GetSelectedName()
        {
            string selectedName = "";

            this.Dispatcher.Invoke(() =>
            {
                selectedName = (DG_Agents.SelectedItem as GridViewSources.Agent).Name;
            });

            return selectedName;
        }

        private string GetSelectedSurname()
        {
            string selectedSurname = "";

            this.Dispatcher.Invoke(() =>
            {
                selectedSurname = (DG_Agents.SelectedItem as GridViewSources.Agent).Surname;
            });

            return selectedSurname;
        }

        private string GetSelectedEmail()
        {
            string selectedEmail= "";

            this.Dispatcher.Invoke(() =>
            {
                selectedEmail = (DG_Agents.SelectedItem as GridViewSources.Agent).Email;
            });

            return selectedEmail;
        }


    }
}
