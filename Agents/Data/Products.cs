//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agents.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Realisation = new HashSet<Realisation>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Article { get; set; }
        public int AmountPeople { get; set; }
        public int ShopNumber { get; set; }
        public decimal MinimumPrice { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Realisation> Realisation { get; set; }
    }
}
