using NetUtil.Util.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter.Attributes
{
    /// <summary>
    /// Define a restricao de uma propriedade na consulta
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RestrictionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Restriction? Restriction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Initialize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Order? Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Construtor que considera a restricao 'Equal' como default e nao inicializa a propriedade
        /// </summary>
        /// <param name="property"></param>
        public RestrictionAttribute(string property)
        {
            this.Property = property;
            this.Restriction = Enums.Restriction.Eq;
            this.Initialize = false;
            this.Order = null;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao e nao inicializa a propriedade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Restriction restriction)
        {
            this.Property = property;
            this.Restriction = restriction;
            this.Initialize = false;
            this.Order = null;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao e indica se a propriedade deve ser inicializada pós consulta
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Restriction restriction, bool initialize)
        {
            this.Property = property;
            this.Restriction = restriction;
            this.Initialize = initialize;
            this.Order = null;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao e indica se a propriedade deve ser inicializada pós consulta
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, bool initialize)
        {
            this.Property = property;
            this.Initialize = initialize;
            this.Restriction = null;
            this.Order = null;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao e ordenacao sem prioridade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Restriction restriction, Order order)
        {
            this.Property = property;
            this.Restriction = restriction;
            this.Order = order;
            this.Initialize = false;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao e ordenacao com prioridade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Restriction restriction, Order order, int priority)
        {
            this.Property = property;
            this.Restriction = restriction;
            this.Order = order;
            this.Priority = priority;
            this.Initialize = false;
        }

        /// <summary>
        /// Construtor que especifica a restricao de ordenacao com prioridade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Order order, int priority)
        {
            this.Property = property;
            this.Order = order;
            this.Priority = priority;
            this.Restriction = null;
            this.Initialize = false;
        }

        /// <summary>
        /// Construtor que especifica a restricao e ordenacao sem prioridade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Order order)
        {
            this.Property = property;
            this.Order = order;
            this.Restriction = null;
            this.Initialize = false;
            this.Priority = 0;
        }

        /// <summary>
        /// Construtor que especifica a restricao, indica se a propriedade deve ser inicializado pós consulta
        ///  e ordenacao com prioridade
        /// </summary>
        /// <param name="property"></param>
        /// <param name="restriction"></param>
        public RestrictionAttribute(string property, Restriction restriction, Order order, int priority, bool initialize)
        {
            this.Property = property;
            this.Restriction = restriction;
            this.Order = order;
            this.Priority = priority;
            this.Initialize = initialize;
        }

    }
}
