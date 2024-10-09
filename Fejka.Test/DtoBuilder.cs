using System;

namespace Fejka.Test;

public static class DtoBuilder
{
    public static TDto Create<TDto>(Action<TDto> defaultAction, Action<TDto> customizeAction = null)
    {
        var dto = Activator.CreateInstance<TDto>();
        defaultAction(dto);
        customizeAction?.Invoke(dto);
        return dto;
    }
}