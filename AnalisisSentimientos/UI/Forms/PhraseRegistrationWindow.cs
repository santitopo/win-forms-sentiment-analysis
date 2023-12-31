﻿using Domain;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class PhraseRegistrationWindow : Form
    {
        private PhraseLogic subsystemPhrase;
        private AnalysisLogic subsystemAnalysis;
        private AlarmLogic subsystemAlarms;
        private AuthorLogic subsystemAuthors;
        private Button alarmNotification;


        public PhraseRegistrationWindow(PhraseLogic s1,AnalysisLogic s2, AlarmLogic s3,
                                            AuthorLogic s4, Button AlarmsButton)
        {
            InitializeComponent();
            subsystemPhrase = s1;
            subsystemAnalysis = s2;
            subsystemAlarms = s3;
            subsystemAuthors = s4;
            LoadComponents();
            alarmNotification = AlarmsButton;
        }

        private void LoadComponents()
        {
            Author[] authors = subsystemAuthors.GetAuthors;
            foreach (Author a in authors)
            {
                cbxAuthors.Items.Add(a.Username);
            }
        }

        private void btnRegisterPhrase_Click(object sender, EventArgs e)
        {
            if (AreEmptyFields())
            {
                MessageBox.Show("No pueden haber campos vacíos");
            }
            else if (InvalidDate())
            {
                MessageBox.Show("Fecha inválida, la fecha debe ser igual o anterior al dia actual");
            }
            else
            {
                Phrase phrase = new Phrase()
                {
                    Content = txtPhrase.Text,
                    Date = dtpDate.Value,
                    Author = subsystemAuthors.GetAuthors[cbxAuthors.SelectedIndex]
                };

                subsystemPhrase.AddPhrase(phrase);

                Analysis phraseAnalysis = subsystemAnalysis.ExecuteAnalysis(phrase);
                subsystemAnalysis.AddAnalysis(phraseAnalysis);

                verifyAlarmsAndNotify();

                EmptyFields();
            }
            
        }

        private void verifyAlarmsAndNotify()
        {
            int onAlarmsBeforeVerify = countActivatedAlarms(subsystemAlarms.GetAlarms);
            subsystemAlarms.VerifyAllAlarms();
            int onAlarmsAfterVerify = countActivatedAlarms(subsystemAlarms.GetAlarms);

            if (onAlarmsAfterVerify > onAlarmsBeforeVerify)
            {
                notifyActivatedAlarm();
            }
        }

        private void notifyActivatedAlarm()
        {
            alarmNotification.BackColor = Color.Red;
        }
        private int countActivatedAlarms(Alarm[] alarms)
        {
            int count = 0;
            foreach (Alarm al in alarms){
                if (al.State)
                {
                    count++;
                }
            }
            return count;
        }

        private bool InvalidDate()
        {
            return dtpDate.Value > DateTime.Now;
        }

        private bool AreEmptyFields()
        {
            return txtPhrase.Text.Equals("") || cbxAuthors.SelectedIndex == -1; ;
        }
        private void EmptyFields()
        {
            txtPhrase.Text = "";
            dtpDate.Value = DateTime.Now;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
