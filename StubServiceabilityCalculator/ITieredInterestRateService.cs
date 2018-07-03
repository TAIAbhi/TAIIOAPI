using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BeamWcfWebService;
namespace StubServiceabilityCalculator
{ 
    [ServiceContract]
    public interface ITieredInterestRateService
    {
        
        
        [OperationContract]
        [WebInvoke(UriTemplate = "GetInterestRateList",
           Method = "POST")]
        InterestRateResponse GetInterestRateList(InterestRateRequest Request);
    }
}
namespace BeamWcfWebService
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "InterestRateRequest", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public partial class InterestRateRequest : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string ProductNameField;

        private string SubproductNameField;

        private string ProductCampaignField;

        private BeamWcfWebService.BrokerageType BrokerageTypeField;

        private int DealerIDField;

        private double AmountFinancedField;

        private BeamWcfWebService.Brand BrandField;

        private int RequestIDField;

        private System.Guid CorrelationIDField;

        private BeamWcfWebService.TermList[] TermListGroupField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string ProductName
        {
            get
            {
                return this.ProductNameField;
            }
            set
            {
                this.ProductNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string SubproductName
        {
            get
            {
                return this.SubproductNameField;
            }
            set
            {
                this.SubproductNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public string ProductCampaign
        {
            get
            {
                return this.ProductCampaignField;
            }
            set
            {
                this.ProductCampaignField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public BeamWcfWebService.BrokerageType BrokerageType
        {
            get
            {
                return this.BrokerageTypeField;
            }
            set
            {
                this.BrokerageTypeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int DealerID
        {
            get
            {
                return this.DealerIDField;
            }
            set
            {
                this.DealerIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public double AmountFinanced
        {
            get
            {
                return this.AmountFinancedField;
            }
            set
            {
                this.AmountFinancedField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public BeamWcfWebService.Brand Brand
        {
            get
            {
                return this.BrandField;
            }
            set
            {
                this.BrandField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int RequestID
        {
            get
            {
                return this.RequestIDField;
            }
            set
            {
                this.RequestIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public System.Guid CorrelationID
        {
            get
            {
                return this.CorrelationIDField;
            }
            set
            {
                this.CorrelationIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public BeamWcfWebService.TermList[] TermListGroup
        {
            get
            {
                return this.TermListGroupField;
            }
            set
            {
                this.TermListGroupField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BrokerageType", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public enum BrokerageType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Internal = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        External = 1,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Brand", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public enum Brand : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        BMW = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mini = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Alphabet = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Alphera = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Motorrad = 4,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TermList", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public partial class TermList : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int TermField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int Term
        {
            get
            {
                return this.TermField;
            }
            set
            {
                this.TermField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "InterestRateResponse", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public partial class InterestRateResponse : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int RequestIDField;

        private System.Guid CorrelationIDField;

        private bool SuccessFlagField;

        private BeamWcfWebService.InterestList[] InterestListGroupField;

        private BeamWcfWebService.ErrorList[] ErrorListGroupField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int RequestID
        {
            get
            {
                return this.RequestIDField;
            }
            set
            {
                this.RequestIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public System.Guid CorrelationID
        {
            get
            {
                return this.CorrelationIDField;
            }
            set
            {
                this.CorrelationIDField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public bool SuccessFlag
        {
            get
            {
                return this.SuccessFlagField;
            }
            set
            {
                this.SuccessFlagField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public BeamWcfWebService.InterestList[] InterestListGroup
        {
            get
            {
                return this.InterestListGroupField;
            }
            set
            {
                this.InterestListGroupField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public BeamWcfWebService.ErrorList[] ErrorListGroup
        {
            get
            {
                return this.ErrorListGroupField;
            }
            set
            {
                this.ErrorListGroupField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "InterestList", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public partial class InterestList : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int TermField;

        private double MinInterestRateField;

        private double MaxInterestRateField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int Term
        {
            get
            {
                return this.TermField;
            }
            set
            {
                this.TermField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public double MinInterestRate
        {
            get
            {
                return this.MinInterestRateField;
            }
            set
            {
                this.MinInterestRateField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public double MaxInterestRate
        {
            get
            {
                return this.MaxInterestRateField;
            }
            set
            {
                this.MaxInterestRateField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ErrorList", Namespace = "http://schemas.datacontract.org/2004/07/BeamWcfWebService")]
    public partial class ErrorList : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string ErrorMessageField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                this.ErrorMessageField = value;
            }
        }
    }
}


