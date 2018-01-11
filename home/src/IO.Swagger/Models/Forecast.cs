/*
 * home
 *
 * The API for the Home Starter project
 *
 * OpenAPI spec version: 1.0.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Forecast :  IEquatable<Forecast>
    { 
        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        [DataMember(Name="date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or Sets Pressure
        /// </summary>
        [DataMember(Name="pressure")]
        public double? Pressure { get; set; }

        /// <summary>
        /// Gets or Sets Humidity
        /// </summary>
        [DataMember(Name="humidity")]
        public int? Humidity { get; set; }

        /// <summary>
        /// Gets or Sets WindSpeed
        /// </summary>
        [DataMember(Name="windSpeed")]
        public double? WindSpeed { get; set; }

        /// <summary>
        /// Gets or Sets Clouds
        /// </summary>
        [DataMember(Name="clouds")]
        public int? Clouds { get; set; }

        /// <summary>
        /// Gets or Sets Temperature
        /// </summary>
        [DataMember(Name="temperature")]
        public ForecastTemperature Temperature { get; set; }

        /// <summary>
        /// Gets or Sets Weather
        /// </summary>
        [DataMember(Name="weather")]
        public WeatherForecast Weather { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Forecast {\n");
            sb.Append("  Date: ").Append(Date).Append("\n");
            sb.Append("  Pressure: ").Append(Pressure).Append("\n");
            sb.Append("  Humidity: ").Append(Humidity).Append("\n");
            sb.Append("  WindSpeed: ").Append(WindSpeed).Append("\n");
            sb.Append("  Clouds: ").Append(Clouds).Append("\n");
            sb.Append("  Temperature: ").Append(Temperature).Append("\n");
            sb.Append("  Weather: ").Append(Weather).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Forecast)obj);
        }

        /// <summary>
        /// Returns true if Forecast instances are equal
        /// </summary>
        /// <param name="other">Instance of Forecast to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Forecast other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Date == other.Date ||
                    Date != null &&
                    Date.Equals(other.Date)
                ) && 
                (
                    Pressure == other.Pressure ||
                    Pressure != null &&
                    Pressure.Equals(other.Pressure)
                ) && 
                (
                    Humidity == other.Humidity ||
                    Humidity != null &&
                    Humidity.Equals(other.Humidity)
                ) && 
                (
                    WindSpeed == other.WindSpeed ||
                    WindSpeed != null &&
                    WindSpeed.Equals(other.WindSpeed)
                ) && 
                (
                    Clouds == other.Clouds ||
                    Clouds != null &&
                    Clouds.Equals(other.Clouds)
                ) && 
                (
                    Temperature == other.Temperature ||
                    Temperature != null &&
                    Temperature.Equals(other.Temperature)
                ) && 
                (
                    Weather == other.Weather ||
                    Weather != null &&
                    Weather.Equals(other.Weather)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Date != null)
                    hashCode = hashCode * 59 + Date.GetHashCode();
                    if (Pressure != null)
                    hashCode = hashCode * 59 + Pressure.GetHashCode();
                    if (Humidity != null)
                    hashCode = hashCode * 59 + Humidity.GetHashCode();
                    if (WindSpeed != null)
                    hashCode = hashCode * 59 + WindSpeed.GetHashCode();
                    if (Clouds != null)
                    hashCode = hashCode * 59 + Clouds.GetHashCode();
                    if (Temperature != null)
                    hashCode = hashCode * 59 + Temperature.GetHashCode();
                    if (Weather != null)
                    hashCode = hashCode * 59 + Weather.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Forecast left, Forecast right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Forecast left, Forecast right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
