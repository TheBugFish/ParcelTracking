/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 2.2.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Runtime.Serialization;

namespace SKS.ParcelLogistics.WebService.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Truck : IEquatable<Truck>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Truck" /> class.
        /// </summary>
        /// <param name="Code">Code (required).</param>
        /// <param name="NumberPlate">NumberPlate (required).</param>
        /// <param name="Latitude">Latitude (required).</param>
        /// <param name="Longitude">Longitude (required).</param>
        /// <param name="Radius">Radius (required).</param>
        /// <param name="Duration">Duration (required).</param>
        public Truck(string Code = default(string), string NumberPlate = default(string), decimal? Latitude = default(decimal?), decimal? Longitude = default(decimal?), decimal? Radius = default(decimal?), decimal? Duration = default(decimal?))
        {
            this.Code = Code;
            this.NumberPlate = NumberPlate;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Radius = Radius;
            this.Duration = Duration;
        }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }
        /// <summary>
        /// Gets or Sets NumberPlate
        /// </summary>
        [DataMember(Name = "numberPlate")]
        public string NumberPlate { get; set; }
        /// <summary>
        /// Gets or Sets Latitude
        /// </summary>
        [DataMember(Name = "latitude")]
        public decimal? Latitude { get; set; }
        /// <summary>
        /// Gets or Sets Longitude
        /// </summary>
        [DataMember(Name = "longitude")]
        public decimal? Longitude { get; set; }
        /// <summary>
        /// Gets or Sets Radius
        /// </summary>
        [DataMember(Name = "radius")]
        public decimal? Radius { get; set; }
        /// <summary>
        /// Gets or Sets Duration
        /// </summary>
        [DataMember(Name = "duration")]
        public decimal? Duration { get; set; }


        /// <summary>
        /// Returns true if Truck instances are equal
        /// </summary>
        /// <param name="other">Instance of Truck to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Truck other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) &&
                (
                    this.NumberPlate == other.NumberPlate ||
                    this.NumberPlate != null &&
                    this.NumberPlate.Equals(other.NumberPlate)
                ) &&
                (
                    this.Latitude == other.Latitude ||
                    this.Latitude != null &&
                    this.Latitude.Equals(other.Latitude)
                ) &&
                (
                    this.Longitude == other.Longitude ||
                    this.Longitude != null &&
                    this.Longitude.Equals(other.Longitude)
                ) &&
                (
                    this.Radius == other.Radius ||
                    this.Radius != null &&
                    this.Radius.Equals(other.Radius)
                ) &&
                (
                    this.Duration == other.Duration ||
                    this.Duration != null &&
                    this.Duration.Equals(other.Duration)
                );
        }
    }
}