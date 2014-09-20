using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class SimulationManager
    {
        public Purchase PurchaseSingleItem(StoreItem item, Student stud, Account acct, Account savings)
        {
            if ((item.EnergyChange >= 0) || CanCompleteAction(stud.Energy, item.EnergyChange))
            {

                var purchase = new Purchase { PurchaseDate = DateTime.Now.Date, StoreItemName = item.StoreItemName, ItemPrice = item.ItemPrice, StoreName = item.Store.StoreName, StudentID = stud.UserId };

                stud.Energy += item.EnergyChange;
                stud.Happiness += item.HappinessChange;
                stud.Health += item.HealthChange;
                stud.Hunger += item.HungerChange;
                stud.Social += item.SocialChange;

                if (stud.Energy > 100) stud.Energy = 100;
                if (stud.Happiness > 100) stud.Happiness = 100;
                if (stud.Health > 100) stud.Health = 100;
                if (stud.Hunger > 100) stud.Hunger = 100;
                if (stud.Social > 100) stud.Social = 100;

                if (stud.Energy < 0) stud.Energy = 0;
                if (stud.Happiness < 0) stud.Happiness = 0;
                if (stud.Health < 0) stud.Health = 0;
                if (stud.Hunger < 0) stud.Hunger = 0;
                if (stud.Social < 0) stud.Social = 0;

                if (acct.AccountTotal < item.ItemPrice)
                {
                    decimal remainder = Math.Abs(acct.AccountTotal - item.ItemPrice);

                    if (savings.AccountTotal >= remainder)
                    {
                        acct.AccountTotal = 0;
                        savings.AccountTotal -= remainder;
                        savings.AccountTotal -= 5; //Overdraft
                        stud.bBillsPaid = false;
                    }
                    else if (savings.AccountTotal < remainder)
                    {
                        savings.AccountTotal -= remainder;
                        savings.AccountTotal -= 28; //Overdraft
                        //stud.bBillsPaid = false;
                    }
                }
                else acct.AccountTotal -= item.ItemPrice;

                return purchase;
            }
            else return null;
        }

        //TODO: Add up the price of all items then do overdrafts at the very end instead of after every item
        public ICollection<Purchase> PurchaseMultipleItems(ICollection<StoreItem> items, Student stud, Account acct, Account savings)
        {

            ICollection<Purchase> purchases = new List<Purchase>();

            foreach (var storeItem in items)
            {
                if ((storeItem.EnergyChange >= 0) || CanCompleteAction(stud.Energy, storeItem.EnergyChange))
                {

                    var purchase = new Purchase { PurchaseDate = DateTime.Now.Date, StoreItemName = storeItem.StoreItemName, ItemPrice = storeItem.ItemPrice, StoreName = storeItem.Store.StoreName, StudentID = stud.UserId };
                    purchases.Add(purchase);

                    stud.Energy += storeItem.EnergyChange;
                    stud.Happiness += storeItem.HappinessChange;
                    stud.Health += storeItem.HealthChange;
                    stud.Hunger += storeItem.HungerChange;
                    stud.Social += storeItem.SocialChange;

                    if (stud.Energy > 100) stud.Energy = 100;
                    if (stud.Happiness > 100) stud.Happiness = 100;
                    if (stud.Health > 100) stud.Health = 100;
                    if (stud.Hunger > 100) stud.Hunger = 100;
                    if (stud.Social > 100) stud.Social = 100;

                    if (stud.Energy < 0) stud.Energy = 0;
                    if (stud.Happiness < 0) stud.Happiness = 0;
                    if (stud.Health < 0) stud.Health = 0;
                    if (stud.Hunger < 0) stud.Hunger = 0;
                    if (stud.Social < 0) stud.Social = 0;

                    if (acct.AccountTotal < storeItem.ItemPrice)
                    {
                        decimal remainder = Math.Abs(acct.AccountTotal - storeItem.ItemPrice);

                        if (savings.AccountTotal >= remainder)
                        {
                            acct.AccountTotal = 0;
                            savings.AccountTotal -= remainder;
                            savings.AccountTotal -= 5;
                            //stud.bBillsPaid = false;
                        }
                        else if (savings.AccountTotal < remainder)
                        {
                            savings.AccountTotal -= remainder;
                            savings.AccountTotal -= 28; //Overdraft
                            //stud.bBillsPaid = false;
                        }
                    }
                    else acct.AccountTotal -= storeItem.ItemPrice;
                }
            }
            return purchases; //Maybe add else return null
        }

        public bool CanCompleteAction(decimal studEnergy, decimal itemEnergy)
        {
            if (((studEnergy) + (itemEnergy)) >= 0)
            {
                return true;
            }
            else return false;
        }
    }
}