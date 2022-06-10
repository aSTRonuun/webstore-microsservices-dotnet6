namespace GeekShopping.ProductAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration ResgisterMpas()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductVO, Product>();
            config.CreateMap<Product, ProductVO>();
        });
        return mappingConfig;
    }
}
