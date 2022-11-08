using System.Diagnostics.CodeAnalysis;
using Credfeto.Enumeration.Source.Generation.Attributes;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Services;

[EnumText(typeof(LogLevel))]
[SuppressMessage(category: "ReSharper", checkId: "PartialTypeWithSinglePart", Justification = "Needed for generated code")]
internal static partial class EnumExtensions
{
}