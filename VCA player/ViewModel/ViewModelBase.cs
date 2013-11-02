using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace VCA_player.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            //this.VerifyPropertyName(propertyName);

            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(
                    ViewModelBase.GetPropertyName(propertyExpresssion));
                PropertyChanged(this, e);
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (propertyExpresssion == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpresssion.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(
                    "The expression is not a member access expression.",
                    "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    "The member access expression does not access a property.",
                    "propertyExpression");
            }

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
            {
                throw new ArgumentException(
                    "The referenced property is a static property.",
                    "propertyExpression");
            }

            return memberExpression.Member.Name;
        }
    }
}
