﻿// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.3
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace DataIngest.Entities
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// This record contains a single timestamped weather forecast entry
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.3")]
	public partial class WeatherMessage : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""WeatherMessage"",""doc"":""This record contains a single timestamped weather forecast entry"",""namespace"":""WeatherService"",""fields"":[{""name"":""timestamp"",""doc"":""Timestamp of the entry"",""type"":""string""},{""name"":""temperature"",""doc"":""The temperature in the unit specified by temperature_unit"",""type"":""double""},{""name"":""temperature_unit"",""doc"":""The unit used for the temperature value"",""type"":""string""},{""name"":""pressure"",""doc"":""The pressure in the unit specified by pressure_unit"",""type"":""double""},{""name"":""pressure_unit"",""doc"":""The unit used for the pressure value"",""type"":""string""}]}");
		/// <summary>
		/// Timestamp of the entry
		/// </summary>
		private string _timestamp;
		/// <summary>
		/// The temperature in the unit specified by temperature_unit
		/// </summary>
		private double _temperature;
		/// <summary>
		/// The unit used for the temperature value
		/// </summary>
		private string _temperature_unit;
		/// <summary>
		/// The pressure in the unit specified by pressure_unit
		/// </summary>
		private double _pressure;
		/// <summary>
		/// The unit used for the pressure value
		/// </summary>
		private string _pressure_unit;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return WeatherMessage._SCHEMA;
			}
		}
		/// <summary>
		/// Timestamp of the entry
		/// </summary>
		public string timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				this._timestamp = value;
			}
		}
		/// <summary>
		/// The temperature in the unit specified by temperature_unit
		/// </summary>
		public double temperature
		{
			get
			{
				return this._temperature;
			}
			set
			{
				this._temperature = value;
			}
		}
		/// <summary>
		/// The unit used for the temperature value
		/// </summary>
		public string temperature_unit
		{
			get
			{
				return this._temperature_unit;
			}
			set
			{
				this._temperature_unit = value;
			}
		}
		/// <summary>
		/// The pressure in the unit specified by pressure_unit
		/// </summary>
		public double pressure
		{
			get
			{
				return this._pressure;
			}
			set
			{
				this._pressure = value;
			}
		}
		/// <summary>
		/// The unit used for the pressure value
		/// </summary>
		public string pressure_unit
		{
			get
			{
				return this._pressure_unit;
			}
			set
			{
				this._pressure_unit = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.timestamp;
			case 1: return this.temperature;
			case 2: return this.temperature_unit;
			case 3: return this.pressure;
			case 4: return this.pressure_unit;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.timestamp = (System.String)fieldValue; break;
			case 1: this.temperature = (System.Double)fieldValue; break;
			case 2: this.temperature_unit = (System.String)fieldValue; break;
			case 3: this.pressure = (System.Double)fieldValue; break;
			case 4: this.pressure_unit = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
