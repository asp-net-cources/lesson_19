using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lesson19.Models;

namespace Lesson19.JsonSettings.Converters;

public class ProductJsonConverter : JsonConverter<ProductModel>
{
    public override ProductModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var modelTypeReader = reader;

        if (modelTypeReader.TokenType != JsonTokenType.StartObject) {
            throw new JsonException();
        }

        Data.Models.ProductType? productType = null;
        while (modelTypeReader.Read()) {
            if (modelTypeReader.TokenType == JsonTokenType.PropertyName) {
                var propertyName = modelTypeReader.GetString();
                if (propertyName?.ToLower() == "producttype") {
                    modelTypeReader.Read();
                    var modelType = modelTypeReader.GetString();
                    if (!string.IsNullOrWhiteSpace(modelType)) {
                        Enum.TryParse(modelType!, true, out Data.Models.ProductType parsedType);
                        productType = parsedType;
                    }
                    break;
                }
            }
        }

        ProductModel? product = productType switch {
            Data.Models.ProductType.Accessories => JsonSerializer.Deserialize<AccessoriesModel?>(ref reader, options),
            Data.Models.ProductType.Book => JsonSerializer.Deserialize<BookModel?>(ref reader, options),
            Data.Models.ProductType.Food => JsonSerializer.Deserialize<FoodModel?>(ref reader, options),
            _ => throw new JsonException($"Can't find model discriminator {productType}")
        };

        if (product == null) {
            throw new JsonException($"Parsing exception {productType}");
        }

        return product;
    }

    public override void Write(Utf8JsonWriter writer, ProductModel value, JsonSerializerOptions options) {
        switch (value) {
            case AccessoriesModel accessories:
                JsonSerializer.Serialize(writer, accessories, options);
                break;
            case BookModel book:
                JsonSerializer.Serialize(writer, book, options);
                break;
            case FoodModel food:
                JsonSerializer.Serialize(writer, food, options);
                break;
        }
    }
}
