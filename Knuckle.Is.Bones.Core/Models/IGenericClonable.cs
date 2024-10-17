using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.Core.Models
{
    public interface IGenericClonable<T>
    {
        public T Clone();
    }
}