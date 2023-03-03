using System;

namespace Assets.Scripts.Common.DI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DependuncyInjectionAttribute : Attribute // �̱��� �ڵ忡 ������ �ο��ϱ� 
    {
        public Type ObjectType;
        public DependuncyInjectionAttribute(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}