#EDF Reader Writer

Objective: Be able to convert multiple biosignal/signal data files into the EDF format for easy viewing and analysis

Current Capabilities: Able to convert CNT (continuous) EEG data files into EDF format with default annotations. Produces an EDF+ (latest format specification) compatible file.

**Usage**
1)	Open CNT file in CNT viewer program. Note the number of channels and the total duration of the recording.
2)	 Open the EDFReaderWriter program. Enter relevant details on the left column of boxes. The right column of boxes are the important ones to change
3)	Change “number of signals” to the number of channels that you saw in the CNT viewer.
4)	Change “number of records” to the total time in seconds of the CNT recording (seen the the cnt viewer as well)
5)	Click Proceed. 
6)	The selection box on the top indicates the current signal that you are editing properties for. Note that number of samples per record is the sampling rate for that channel of data. Go through each signal and save their properties before clicking Generate Header. Do not worry if the properties all look the same every time you go back to previous properties, they have been saved if the box at the bottom is green.
7)	Click Generate Header.
8)	In the next window, you can either add signals in Single Signal Mode (means one CNT file to one channel [we take the first channel of the CNT file to correspond to the first EDF channel]) or Multiple Signal Mode, where you indicate the range of signals that the CNT channels will map to. 0 – signal 0 in EDF file. So if you input Start Signal Number as 0 and End Signal Number as 5, if 5 channels were defined in the EDF file, the program will try to extract 5 channels of data from the input CNT file and map them to the EDF file. Click Import to choose the CNT file to import. You can import as many files as you want to as many EDF channels are defined, but do note that multiple writes to the same channel will cause overwrites.
9)	Finally, click Generate File. The timestamped file will appear in the same folder as the EDFReaderWriter executable.

**Improvements**
Automatically analyse CNT file for number of channels and total time of recording
More feedback in UI to user.
Update the current signal information when editing properties and switch to other properties.
