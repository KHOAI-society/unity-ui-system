# K Unity UI System (Experimental)

K Unity UI System is an in-progress Unity package that lets you define color and sprite palettes once and apply them across UI widgets through theme-aware components. It is primarily a personal spare-time experiment, so expect rough edges, missing safety checks, and a rapidly changing API.

## Project Status

- Half-finished pet project: breaking changes are likely and not every use case has been validated.
- Editor tooling is focused on a small set of internal needs (color/sprite palettes and themed `Image` support).
- Documentation and samples are minimal; use the source under `package/` as the authoritative reference.

## What’s Inside

- **Palettes**: `KColorPalette` and `KSpritePalette` ScriptableObjects act as lookup tables for theme tokens.
- **Theme Asset**: `KTheme` bundles palettes and notifies any active themed items when values change.
- **Themed Components**: `KImage` demonstrates how to consume palette entries; extend `KThemedItem` to support other widgets.
- **Custom Inspectors**: Palette editors (`KColorPaletteEditor`, `KSpritePaletteEditor`) make it easy to add entries and preview them.

## Installing the Package

1. Clone or download this repository locally.
2. In your Unity project open **Window → Package Manager → + → Add package from disk…**.
3. Select `unity-ui-system/package/package.json`. Unity will import the `com.khoai.kuisystem` package.
4. Create a `Resources` folder in your project (if it does not already exist) and keep exactly one `KTheme` asset inside—runtime code loads `KTheme.Instance` from there.

> **Note:** Because this repository is not published to a public registry, you must keep it as an embedded/local package for now.

## Tutorial: First Themed UI Element

Follow the steps below to wire a button image to the theme system. The same flow applies to any custom component you build on top of `KThemedItem`.

### 1. Create Color and Sprite Palettes

1. In the Project window choose **Create → UI → KColorPalette** and **Create → UI → KSpritePalette**.
2. Open each asset:
   - Use the custom inspector to add named entries (e.g., “Primary/Default”, “Primary/Hover”, “Icon/Checkmark”).
   - Assign colors or sprites respectively; palette data stays serialized as lists so you can version it.

### 2. Create a Theme Asset

1. Choose **Create → UI → KTheme**.
2. Assign the color and sprite palettes you just created.
3. Move the `KTheme` asset to a folder under `Resources/` (for example `Assets/Resources/UITheme.asset`). Only one instance should exist to satisfy `KTheme.Instance`.

### 3. Hook Up a UI Image

1. Add a standard Unity `Image` to your canvas (or pick an existing one).
2. Add the `KImage` component. It inherits from `KThemedItem` and implements both `KIColorAppliedItem` and `KISpriteAppliedItem`.
3. Enable the `mainColor` toggle and type the exact name of the color entry you want.
4. Enable the `mainSprite` toggle if you want the sprite to be theme-controlled.
5. Enter Play Mode or trigger `OnValidate` (toggling the component in the inspector works) and the `Image` will pull values through `GetColor`/`GetSprite`.

### 4. Update the Theme at Runtime (Optional)

Switching themes simply swaps palette references and syncs active items:

```csharp
using UnityEngine;
using Khoai;

public class ThemeSwitcher : MonoBehaviour
{
    [SerializeField] KTheme runtimeTheme;
    [SerializeField] KColorPalette lightColors;
    [SerializeField] KSpritePalette lightSprites;

    public void ApplyLightTheme()
    {
        runtimeTheme.SetColorPalette(lightColors);
        runtimeTheme.SetSpritePalette(lightSprites);
    }
}
```

Because all themed components (`KImage`, or your own subclasses of `KThemedItem`) register themselves through `KMonoBehaviour`, calling the update methods forces every instance to resync.

### 5. Extend the System (Optional)

To theme other widgets:

1. Create a MonoBehaviour that inherits from `KThemedItem`.
2. Add serialized `KThemedItemProperty` fields marked with `[KColorSelection]` or `[KSpriteSelection]`.
3. Override `SyncColor`/`SyncSprite` to apply palette values to your specific component.
4. Inside `OnValidate` call `SyncColor()` / `SyncSprite()` so editor changes propagate instantly.

```csharp
public class KText : KThemedItem, KIColorAppliedItem
{
    [KColorSelection] [SerializeField] KThemedItemProperty textColor;

    public override void SyncColor(KColorPalette palette)
    {
        base.SyncColor(palette);
        if (!palette || !textColor.use) return;
        GetComponent<TMPro.TMP_Text>().color = GetColor(textColor.itemName);
    }
}
```

## Contributing / Feedback

Issues, ideas, and pull requests are welcome, but keep in mind development happens sporadically. Feel free to fork the project, adapt it to your needs, and share findings through GitHub issues. The best documentation remains the source itself—explore the `package/Scripts` and `package/Editor` folders for reference implementations.
