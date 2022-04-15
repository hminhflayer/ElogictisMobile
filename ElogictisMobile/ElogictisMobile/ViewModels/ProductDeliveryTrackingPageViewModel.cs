using ElogictisMobile.Models;
using ElogictisMobile.Services;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    public class ProductDeliveryTrackingPageViewModel : BaseViewModel
    {
        public ProductDeliveryTrackingPageViewModel()
        {
            ProductDeliveryTrackings = new List<ProductDeliveryTrackingModel>();
            LoadData();

            this.orderId = LocalContext.ProductSelected.ID;
            this.fromName = LocalContext.ProductSelected.From_Address;
            this.toName = LocalContext.ProductSelected.To_Address;
        }

        #region Fields

        private string productName;

        private string description;

        private string fromName;

        private string toName;

        private string status;

        private string orderId;

        private string productImage;

        private List<ProductDeliveryTrackingModel> productDeliveryTrackings;

        #endregion

        #region Properties

        public List<ProductDeliveryTrackingModel> ProductDeliveryTrackings
        {
            get
            {
                return this.productDeliveryTrackings;
            }

            set
            {
                this.SetProperty(ref this.productDeliveryTrackings, value);
            }
        }

        public string ProductImage
        {
            get
            {
                return App.ImageServerPath + this.productImage;
            }

            set
            {
                this.SetProperty(ref this.productImage, value);
            }
        }

        public string ProductName
        {
            get
            {
                return this.productName;
            }

            set
            {
                this.SetProperty(ref this.productName, value);
            }
        }

        public string FromName
        {
            get
            {
                return this.fromName;
            }

            set
            {
                this.SetProperty(ref this.fromName, value);
            }
        }

        public string ToName
        {
            get
            {
                return this.toName;
            }

            set
            {
                this.SetProperty(ref this.toName, value);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.SetProperty(ref this.description, value);
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.SetProperty(ref this.status, value);
            }
        }

        public string OrderID
        {
            get
            {
                return this.orderId;
            }

            set
            {
                this.SetProperty(ref this.orderId, value);
            }
        }

        #endregion

        #region Methods
        public async void LoadData()
        {
            ProductDeliveryTrackings = await RealtimeFirebase.Instance.GetDeliveryTrackingModelsAsync(LocalContext.ProductSelected.ID);
        }
        #endregion
    }
}
