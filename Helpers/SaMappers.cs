using AutoMapper;
using System.Collections.Concurrent;

namespace Hospital.Helpers {
    public static class SaMappers {
        private static readonly ConcurrentDictionary<(Type, Type), IMapper> _mappers = new();

        public static V MapTo<T,V>(this T from, V to){
            var key = (typeof(T), typeof(V));
            var iMapper = _mappers.GetOrAdd(key, _ => {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<T, V>();
                });
                return config.CreateMapper();
            });

            iMapper.Map<T,V>(from,to);
            return to;
        }

        public static int ToInt32(this string text) {
            return Convert.ToInt32(text);
        }
    }
}
