using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace HCI_Projekat.gui
{
    public class PlaceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            //Place place = (value as BindingGroup).Items[0] as Place;
            if (value == null || value.ToString().Trim().Length == 0)
            {
                return new ValidationResult(false,
                    "unesite naziv stanice");
            }
            else if (PlaceService.FindPlaceByName(value.ToString().Trim()) != null)
            {
                return new ValidationResult(false,
                    "stanica sa unetim nazivom već postoji");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
