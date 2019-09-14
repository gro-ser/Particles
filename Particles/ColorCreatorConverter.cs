using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;

namespace Particles
{
    internal class ColorCreatorConverter:TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value.GetType(), attributes);
            return properties;
        }
    }
}