using System;

namespace Assets.Scripts.Common.DI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DependuncyInjectionAttribute : Attribute // 싱글톤 코드에 의존성 부여하기 
    {
        public Type ObjectType;
        public DependuncyInjectionAttribute(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}