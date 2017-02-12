# NPP-Starbound-DescriptorToCommand
A NotePad++ plugin that implements the same functionality as [Starbound-DescriptorToCommand](https://silverfeelin.github.io/Starbound-DescriptorToCommand/):  
An easy method to turn a valid Starbound Item Descriptor into a `/spawnitem` command.

The plugin only works for the 32-bit version of NotePad++.

## Installation
* Download the latest release.
* Ensure NotePad++ is closed.
* Place the `DescriptorToCommand.dll` file in your `plugins` folder (typically "`C:\Program Files (x86)\Notepad++\plugins\`").
* Open NotePad++.

## Usage

The plugin uses the contents of your current document to generate the command. There are two options:

### Copy Command

<kbd>Ctrl</kbd><kbd>Alt</kbd><kbd>Shift</kbd>+<kbd>C</kbd>  
or  
`Plugins > Descriptor to Command > Copy Command`

Copies the command directly to your clipboard (without opening it in a new document).

### Show Command

<kbd>Ctrl</kbd><kbd>Alt</kbd><kbd>Shift</kbd>+<kbd>S</kbd>  
or  
`Plugins > Descriptor to Command > Show Command`

Displays the command in a new document, and selects all text (without copying it to your clipboard).

![](https://raw.githubusercontent.com/Silverfeelin/NPP-Starbound-DescriptorToCommand/master/readme/overview.png)  
*The available options under the Plugins tab*

## Editing Shortcuts

The shortcuts to both functions can be modified in the Shortcut Mapper.

`Tools > Shortcut Mapper... > Plugin commands`

![](https://raw.githubusercontent.com/Silverfeelin/NPP-Starbound-DescriptorToCommand/master/readme/shortcuts.png)

## Licenses

The sources of the below projects are used in this project. I highly suggest you check them out if you're interested in knowing what makes this plugin possible!

Name | Link | License
--- | --- | ---
NotepadPlusPlusPluginPack.Net | [GitHub](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net) | [Apache 2.0](https://github.com/Silverfeelin/NPP-Starbound-DescriptorToCommand/blob/master/DescriptorToCommand/NotepadPlusPlusPluginPack.Net-LICENSE.md)
SimpleJSON.NET | [GitHub](https://github.com/mhallin/SimpleJSON.NET) | [3-Clause BSD](https://github.com/Silverfeelin/NPP-Starbound-DescriptorToCommand/blob/master/DescriptorToCommand/SimpleJSON.NET-LICENSE.md)
