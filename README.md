# Akorin
The OREMO killer: A line-by-line audio recording tool for singing synthesis libraries

![Akorin screenshot](screenshot.png)

Download latest version from [Releases](https://github.com/adlez27/akorin/releases).

## Features
* Load plain text reclists, or Akorin Recording Lists that include notes
* Load Shift JIS and UTF-8 reclists
* Split plain text lists by all whitespace or by newline
* Set a destination folder for auto-saved recordings
* Record from any input or output, and play back through any output
* Set input/output levels
* Customize font size of selected line
* Display waveform and customize color of waveform
* Highlight lines that have already been recorded
* Save ARP (Akorin Recording Project) files
* Set your own default project settings for all new projects

## Installation
**Windows:** Unzip and place the folder anywhere. Run `Akorin.exe` to start Akorin.  
**Mac:** Unzip and place `Akorin.app` anywhere. Control+click to open and start Akorin.

## Usage
### Recording
Press `R` once to start recording, and press `R` again to stop recording. The waveform will appear after recording is stopped.  
Press `space` once to start playback, and press `space` again to stop playing.  
Click on items in the list or use arrow keys to navigate through the list. Audio is automatically saved to your specified folder when moving through the list.

### Changing reclist and folder
Go to Settings > List and folder.

![File settings screenshot](file-settings.png)

Currently supported reclist formats are plain text and ARL. You can check [Projects](https://github.com/adlez27/akorin/projects) to see what other formats are planned.

### Writing an Akorin Recording List
Akorin Recording Lists use YAML, which is very simple to read and write. Create a plain text file using a text editor like Notepad or TextEdit, with the following formatting.
```yaml
line 1: note 1
line 2: note 2
line 3: note 3
```
Save the file with the extension `.arl`. You can refer to the reclists included in the releases for examples of a full reclist. If Akorin has trouble reading the reclist, try surrounding the line or note with double quotes ``"like this"``.

## Other
Suggest new features and submit bug reports in [Issues](https://github.com/adlez27/akorin/issues).  
You can view current plans and progress in [Projects](https://github.com/adlez27/akorin/projects).  
Chat with other devs and users on [Discord](https://discord.gg/qZEPyhSqmf).

---
"File:Font Awesome 5 solid microphone-alt.svg" by Font Awesome is licensed with CC BY 4.0. To view a copy of this license, visit https://creativecommons.org/licenses/by/4.0
