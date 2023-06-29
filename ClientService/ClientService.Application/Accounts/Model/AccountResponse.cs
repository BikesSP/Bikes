using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Accounts.Model
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public String Avatar { get; set; }
        public String Card { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public float AveragePoint { get; set; }
        public ObjectStatus Status { get; set; }

        public AccountResponse(Account account)
        {
            this.Id = account.Id;
            this.Avatar = account.AvartarUlr;
            this.Card = account.Card;
            this.Name = account.Name;
            this.Email = account.Email;
            this.Phone = account.Phone;
            this.Status = account.AccountStatus;
            this.AveragePoint = account.averagePoint;
        }
    }
}
