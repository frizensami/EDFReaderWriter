using System;

using System.Threading;
using System.IO;

namespace BCILib.Amp
{
	/// <summary>
	/// Summary description for EEGCntFile.
	/// </summary>
	public class EEGCntFile {
		private const string NUAMPS_CHN_NAMES = "HEOL,HEOR,Fp1,Fp2,VEOU,VEOL,F7,F3,Fz,F4,F8,FT7,FC3,FCz,FC4,FT8,T3,C3,Cz,C4,T4,TP7,CP3,CPz,CP4,TP8,A1,T5,P3,Pz,P4,T6,A2,O1,Oz,O2,FT9,FT10,PO1,PO2";

		private AmpInfo amp_info = new AmpInfo();

		public EEGCntFile() {
		}

		private string path = null;

		public string FileName {
			get {
				return path;
			}
		}

		public int GetStimCodeByNo(int no) {
			if (no < 0 || no >= stim_code.Length) return 0;
			return stim_code[no];
		}

		public float[,] eeg_data;
		public int[] evt_data;

		public int[] stim_code;
        public int[] stim_pos;
        public int[,] stim_num;
        public int desc_spls = 0;

		public bool ReadCnt(string cntf) {
			FileStream fstream;
			BinaryReader f_reader;
			FileInfo finf;
			try {
				finf = new FileInfo(cntf);
				fstream = finf.OpenRead();
				f_reader = new BinaryReader(fstream);
				path = finf.FullName;
			} catch (Exception) {
				Console.WriteLine("Cannot open cnt file {0}.", cntf);
				return false;
			}

			// read header file
			amp_info.num_chan = f_reader.ReadInt32();
			amp_info.num_evt = f_reader.ReadInt32();
			amp_info.blk_samples = f_reader.ReadInt32();
			amp_info.sampling_rate = f_reader.ReadInt32();
			amp_info.data_size = f_reader.ReadInt32();
			if (amp_info.data_size > 4) amp_info.data_size = 4;
			amp_info.resolution = f_reader.ReadSingle();

			if (amp_info.num_chan <= 0 ||
				amp_info.num_evt != 1 || amp_info.data_size != 4 ||
				amp_info.resolution < 0) {
				Console.WriteLine("Invalid cnt format!");
				f_reader.Close();
				fstream.Close();
				return false;
			}

            // ccwang  - version 1.1
            string magic = "I2REEGCNT";
            long pos = f_reader.BaseStream.Position;
            int spz = amp_info.num_chan;
            char[] buf = new char[magic.Length];
            int n0 = 0;
            for (int i = 0; i < buf.Length; i++) {
                if (amp_info.resolution == 0) {
                    buf[i] = (char)f_reader.ReadSingle();
                } else {
                    buf[i] = (char)f_reader.ReadInt32();
                }
                n0++;
                if (n0 % spz == 0) f_reader.ReadInt32();
            }
            string rword = new string(buf);
            if (string.Compare(rword, magic, true) == 0) {
                int[] vl = new int[3];
                for (int i = 0; i < vl.Length; i++) {
                    if (amp_info.resolution == 0) {
                        vl[i] = (char)f_reader.ReadSingle();
                    } else {
                        vl[i] = (char)f_reader.ReadInt32();
                    }
                    n0++;
                    if (n0 % spz == 0) f_reader.ReadInt32();
                }

                char[] rch = new char[vl[2]];
                for (int i = 0; i < rch.Length; i++) {
                    if (amp_info.resolution == 0) {
                        rch[i] = (char)f_reader.ReadSingle();
                    } else {
                        rch[i] = (char)f_reader.ReadInt32();
                    }
                    n0++;
                    if (n0 % spz == 0) f_reader.ReadInt32();
                }

                int nl = magic.Length + 3 + vl[2];
                nl = (nl + spz - 1) / spz * spz;
                while (n0 < nl) {
                    f_reader.ReadInt32();
                    n0++;
                    if (n0 % spz == 0) f_reader.ReadInt32();
                }
                amp_info.chan_names = new string(rch).Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                desc_spls = n0 / amp_info.num_chan;
            } else {
                f_reader.BaseStream.Seek(pos, SeekOrigin.Begin);
            }

			// data reading buffer
			int last_evt = 0;
			int nspl = (int) (finf.Length - fstream.Position) / 
				((amp_info.num_chan + amp_info.num_evt) * 4);
			eeg_data = new float[amp_info.num_chan, nspl];
			evt_data = new int[nspl];
			int nevt = 0;
			int[] stim_his = new int[256];
			int ncounter = 0;

			for (int ispl = 0; ispl < nspl; ispl++) {
				// read eeg
				for (int j = 0; j < amp_info.num_chan; j++) {
					float fv = 0;
					if (amp_info.resolution == 0) {
						fv = f_reader.ReadSingle();
					} else {
						fv = f_reader.ReadInt32() * amp_info.resolution;
					}
					eeg_data[j, ispl] = fv;
				}

				int evt = (f_reader.ReadInt32() & 0xff);
				evt_data[ispl] = evt == last_evt? 0:evt;
				last_evt = evt;

				evt = evt_data[ispl];
				if (evt != 0) {
					nevt++;
					if (stim_his[evt] == 0) ncounter++;
					stim_his[evt]++;
				}
			}

			f_reader.Close();
			fstream.Close();

			stim_code = new int[nevt];
			stim_pos = new int[nevt];
			int ievt = 0;
			for (int ispl = 0; ispl < nspl; ispl++) {
				if (evt_data[ispl] != 0) {
					stim_code[ievt] = evt_data[ispl];
					stim_pos[ievt] = ispl;
					ievt++;
				}
			}

			stim_num = new int[ncounter, 2];
			ievt = 0;
			for (int ic = 0; ic < stim_his.Length; ic++) {
				if (stim_his[ic] > 0) {
					stim_num[ievt, 0] = ic;
					stim_num[ievt, 1] = stim_his[ic];
					ievt++;
				}
			}

			// select channel
			float[,] eeg_r = new float[amp_info.num_chan, 2];
			int sw = amp_info.sampling_rate;
			if (sw > nspl) sw = nspl;
			int x0 = nspl / 2 - sw / 2;
			int x1 = x0 + sw;
			for (int ich = 0; ich < amp_info.num_chan; ich++) {
				eeg_r[ich, 0] = eeg_r[ich, 1] = eeg_data[ich, x0];
			}

			for (int ispl = x0 + 1; ispl < x1; ispl++) {
				for (int ich = 0; ich < amp_info.num_chan; ich++) {
					float fv = eeg_data[ich, ispl];
					if (fv < eeg_r[ich, 0]) eeg_r[ich, 0] = fv;
					if (fv > eeg_r[ich, 1]) eeg_r[ich, 1] = fv;
				}
			}

			int nsel = 0;
			bool[] sidx = new bool[amp_info.num_chan];
			for (int ic = 0; ic < amp_info.num_chan; ic++) {
				float r = eeg_r[ic, 1] - eeg_r[ic, 0];
				if (r < 10) {
					Console.WriteLine("Channel {0} range {1} - {2} discarded.",
						ic, eeg_r[ic, 0], eeg_r[ic, 1]);
					sidx[ic] = false;
				} else {
					nsel++;
					sidx[ic] = true;
				}
			}

			amp_info.valid_idx = new int[nsel];
			nsel = 0;
			for (int ic = 0; ic < amp_info.num_chan; ic++) {
				if (sidx[ic]) {
					amp_info.valid_idx[nsel] = ic;
					nsel++;
				}
			}

			if (amp_info.chan_names == null && amp_info.num_chan == 40) { //NUAmps
				string temp = NUAMPS_CHN_NAMES;
				string[] nl = temp.Split(new char[] {','}, amp_info.num_chan);
				amp_info.chan_names = new string[40];
				for (int i = 0; i < nl.Length; i++)
					amp_info.chan_names[i] = nl[i].Trim();
			}

			return true;
		}

        public AmpInfo Amp_Info
        {
            get
            {
                return amp_info;
            }

            set
            {
                amp_info = value;
            }
        }

		public int GetNumberOfStimcode(int code) {
			for (int i = 0; i < stim_num.GetLength(0); i++) {
				if (stim_num[i, 0] == code) return stim_num[i, 1];
			}
			return 0;
		}

		public float[,,] GetEpochByStimCode(int code, int time_start, int time_end) {
			int nstim = GetNumberOfStimcode(code);
			if (nstim == 0) return null;

			int ep_start = time_start * amp_info.sampling_rate / 1000;
			int ep_end = time_end * amp_info.sampling_rate / 1000;
			float[,,] epoch_data = new float[nstim, amp_info.num_chan, ep_end - ep_start + 1];
			int icounter = 0;
			for (int istim = 0; istim < stim_code.Length; istim++) {
				if (stim_code[istim] != code) continue;
				for (int ichan = 0; ichan < amp_info.num_chan; ichan++) {
					for (int ispl = ep_start; ispl <= ep_end; ispl++) {
						epoch_data[icounter, ichan, ispl - ep_start] =
							eeg_data[ichan, stim_pos[istim] + ispl];
					}
				}
				icounter++;
			}
			return epoch_data;
		}

		public void GetEpochByStimNo(float[,]xbuf,
			int no, int spl_start, int spl_end, int[] sel_idx)
		{
			int nspl = eeg_data.GetLength(1);
			int offset = nspl + nspl;
			if (no >= 0 && no < stim_pos.Length) offset = stim_pos[no];

			for (int ich = 0; ich < sel_idx.Length; ich++) {
				int ichan = sel_idx[ich];
				for (int ispl = spl_start; ispl <= spl_end; ispl++) {
					int ip =  ispl + offset;
					if (ip >= 0 && ip < nspl) {
						xbuf[ich, ispl - spl_start] = eeg_data[ichan, ip];
					} else {
						xbuf[ich, ispl - spl_start] = 0;
					}
				}
			}
		}

		public int NumSamples {
			get {
				return eeg_data.GetLength(1);
			}
		}

		public int[,] StimNumbers {
			get {
				return this.stim_num;
			}
		}
	}

	public class AmpInfo {
		public int num_chan = 0;
		public int num_evt = 0;
		public int blk_samples = 0;
		public int sampling_rate = 0;
		public int data_size = 0;
		public float resolution = 0;

		public string[] chan_names;
		public int[] used_idx;
		public int[] valid_idx;
	};
}
