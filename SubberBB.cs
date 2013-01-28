#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// Enter the description of your strategy here
    /// </summary>
    [Description("Enter the description of your strategy here")]
    public class SubberBB : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
		private int adxPeriod = 14;
		private int adxTrigger = 25;
		private int bollPeriod = 20;
		private double bollDev = 2.0;
		
		//Stop Loss und Take Profit
		private int initialSL = 120;
		private int initialTP = 28;
		
		
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            addIndicatorPlots();
			//addTimeframes();
			
            SetStopLoss("", CalculationMode.Ticks, initialSL, false);
            SetProfitTarget("", CalculationMode.Ticks, initialTP);

            CalculateOnBarClose = false;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            double currentADX = ADX(adxPeriod)[0];
			double barLowPrice = Low[1];
			// Condition set 1
            if (Close[1] < Bollinger(bollDev, bollPeriod).Lower[0]
                && CrossAbove(Close, Bollinger(bollDev, bollPeriod).Lower, 1))
				//&& currentADX <= adxTrigger)
            {
                EnterLong(DefaultQuantity, "");
			}
			
			 if (Close[1] > Bollinger(bollDev, bollPeriod).Upper[0]
                && CrossBelow(Close, Bollinger(bollDev, bollPeriod).Upper, 1))
				//&& currentADX <= adxTrigger)
            {
                EnterShort(DefaultQuantity, "");
			}
			
			
			//5SetStopLoss("", CalculationMode.Price, barLowPrice, false);
			
        }
		
		
		protected void  addIndicatorPlots()
		{
			Add(Bollinger(bollDev, bollPeriod));
			Add(ADX(adxPeriod));
			Add(EMA(50));
			Add(SMA(200));
		}
		
		protected void addTimeframes()
		{
		   Add(PeriodType.Minute,5);
		   Add(PeriodType.Minute,15);
	    }
			
			

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int MyInput0
        {
            get { return myInput0; }
            set { myInput0 = Math.Max(1, value); }
        }
		
		[Description("")]
        [GridCategory("Parameters")]
		public int InitialSL
        {
            get { return initialSL; }
            set { initialSL = Math.Max(1, value); }
        }
		[Description("")]
        [GridCategory("Parameters")]
		public int InitialTP
        {
            get { return initialTP; }
            set { initialTP = Math.Max(1, value); }
        }
        #endregion
    }
}
