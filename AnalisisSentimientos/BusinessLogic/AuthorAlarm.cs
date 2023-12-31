﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumType = Domain.Analysis.Type;

namespace Domain
{
    public class AuthorAlarm : Alarm
    {
        public List<Author> AssociatedAuthors { get; }

        public AuthorAlarm() 
        {
            AssociatedAuthors = new List<Author>();
        }
        public AuthorAlarm(int postNum, bool type, int time) : base(postNum, type, time)
        {
            AssociatedAuthors = new List<Author>();
        }

        public override void VerifyAlarm(Analysis[] analysis, Author[] authors)
        {
            Tuple<Author, int>[] authorIncidence = InitializeList(authors);

            for (int j = 0; j < analysis.Count(); j++)
            {
                Analysis actualAnalysis = analysis[j];
                Author phraseAuthor = actualAnalysis.Phrase.Author;
                DateTime phraseEntryDate = actualAnalysis.Phrase.Date;

                if (ValidDateRange(phraseEntryDate, this.TimeBack) & Match(actualAnalysis,this))
                {
                    IncreaseIncidenceAuthor(authorIncidence, phraseAuthor, PostNumber);
                }
            }

            CheckAlarm();
        }

        public override void CheckAlarm()
        {
            if (AssociatedAuthors.Count > 0)
            {
                State = true;
            }
        }

        private bool Match(Analysis anAnalysis, AuthorAlarm anAlarm)
        {
            var phraseType = anAnalysis.PhraseType;
            if (phraseType == EnumType.neutral)
            {
                return false;
            }
            else
            {
                //We have to refactor the Enum into a bool to compare
                bool phraseFeeling = phraseType == EnumType.positive ? true : false;
                return phraseFeeling.Equals(anAlarm.Type);
            }

        }

        private Tuple<Author, int>[] InitializeList(Author[] authors)
        {
            int size = authors.Length;

            Tuple<Author, int>[] authorIncidence = new Tuple<Author, int>[size];

            for (int i = 0; i < size; i++)
            {
                Author a = authors[i];
                authorIncidence[i] = new Tuple<Author, int>(a, 0);
            }

            return authorIncidence;
        }

        private void AddAuthorToAlarm(Author anAuthor)
        {
            if (!AssociatedAuthors.Contains(anAuthor))
            {
                AssociatedAuthors.Add(anAuthor);
            }
            
        }

        public Author[] GetAsocciatedAuthors()
        {
            return AssociatedAuthors.ToArray();
        }

        private void IncreaseIncidenceAuthor(Tuple<Author, int>[] list, Author author, int MaxPostNumber)
        {

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Item1.Equals(author))
                {
                    int previousValue = list[i].Item2;
                    list[i] = new Tuple<Author, int>(author, previousValue+1);

                    if (list[i].Item2 == MaxPostNumber)
                    {
                        AddAuthorToAlarm(author);
                    }
                }

                
            }

        }

        public override void ResetCounter()
        {
            AssociatedAuthors.Clear();
            State = false;
        }


    }
}
