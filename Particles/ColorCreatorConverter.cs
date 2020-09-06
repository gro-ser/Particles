using System;
using System.ComponentModel;

namespace Particles
{
    internal class ColorCreatorConverter : TypeConverter
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