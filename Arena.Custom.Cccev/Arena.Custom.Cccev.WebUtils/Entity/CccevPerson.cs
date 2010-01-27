using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arena.Custom.Cccev.WebUtils.Entity
{
    [Serializable]
    public class CccevPerson
    {
        private Arena.Core.Person person;

        public string FirstName { get { return person.FirstName; } }
        public string LastName { get { return person.LastName; } }
        public string FullName { get { return person.FullName; } }
        public string Gender { get { return person.Gender.ToString(); } }
        public string MaritalStatus { get { return person.MaritalStatus.Value; } }
        public string BirthDate { get { return person.BirthDate.ToString(); } }
        public string Address { get { return person.Addresses.PrimaryAddress().Address.ToString(); } }
        public string HomePhone { get { return person.Phones.FindByType(Arena.Core.SystemLookup.PhoneType_Home).Number; } }
        public string Email { get { return person.Emails.FirstActive; } }
        public string RecordStatus { get { return person.RecordStatus.ToString(); } }
        public string PhotoHtml { get { return person.PhotoIconHTML; } }

        public string Campus 
        {
            get 
            {
                if (person.Campus != null && person.Campus.CampusId != -1)
                {
                    return person.Campus.Name;
                }
                else
                {
                    return "null campus";
                }
            } 
        }

        public CccevPerson()
        {
            person = new Arena.Core.Person();
        }

        public CccevPerson(int personID)
        {
            person = new Arena.Core.Person(personID);
        }
    }
}
