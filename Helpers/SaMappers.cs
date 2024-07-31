using AutoMapper;

namespace Hospital.Helpers {
    public static class SaMappers {
        public static V MapTo<T,V>(this T from, V to){
            var config = new MapperConfiguration(config => {
                config.CreateMap<T,V>();
            });

            IMapper iMapper = config.CreateMapper();
            iMapper.Map<T,V>(from,to);
            return to;
        }

        public static int ToInt32(this string text) {
            return Convert.ToInt32(text);
        }
    }
}