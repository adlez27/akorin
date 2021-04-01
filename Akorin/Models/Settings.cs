using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ManagedBass;
using ReactiveUI;
using YamlDotNet.Serialization;

namespace Akorin.Models
{
    public class Settings : ISettings
    {
        private bool init = false;
        private string defaultRecListRaw = @"- Text: ""p_aa_r_k_d_aa_r_y_aa_l""
  Note: ""park dar y'all""
- Text: ""k_aa_r_v_w_aa_ch_l_aa_r_jh""
  Note: ""carve watch large""
- Text: ""z_aa_n_ch_aa_r_m_f_aa_r""
  Note: ""zawn charm far""
- Text: ""g_aa_t_hh_aa_r_t_t_aa_n""
  Note: ""got heart tawn""
- Text: ""aa_r_m_d_jh_aa_k_hh_aa_s""
  Note: ""armed jock hoss""
- Text: ""m_aa_b_s_aa_l_v_s_aa_f_t""
  Note: ""mob solve soft""
- Text: ""l_aa_r_m_w_aa_d_d_r_aa_p""
  Note: ""larm wad drop""
- Text: ""n_aa_k_q_aa_z_b_aa_m""
  Note: ""knock 'oz balm""
- Text: ""s_k_ae_n_dh_ae_t_m_ae_n_s""
  Note: ""scan that mance""
- Text: ""hh_ae_v_b_ae_k_t_ae_d_z""
  Note: ""have back tads""
- Text: ""l_ae_f_s_g_r_ae_s_y_ae_m""
  Note: ""laughs grass yam""
- Text: ""jh_ae_ng_f_ae_k_t_th_ae_ng_k_s""
  Note: ""jang fact thanks""
- Text: ""s_ae_ng_y_ae_m_k_q_ae_sh_t""
  Note: ""sang yamk 'ashed""
- Text: ""d_ae_l_y_ae_ng_s_k_ae_l_p""
  Note: ""dal yang scalp""
- Text: ""g_l_ae_n_n_ae_p_ae_z""
  Note: ""glan nap as""
- Text: ""k_ah_m_z_p_ah_r_ah_sh""
  Note: ""comes pah rush""
- Text: ""t_ah_ng_ah_hh_ah_n_t""
  Note: ""tongue a hunt""
- Text: ""p_r_ah_w_ah_s_m_ah_ng""
  Note: ""prah wuss mung""
- Text: ""ch_ah_ng_b_ah_th_ah_n""
  Note: ""chung bah thun""
- Text: ""g_ah_dh_ah_jh_ah_n_t""
  Note: ""gah the junt""
- Text: ""z_ah_m_n_y_ah_d_ah_ch""
  Note: ""zum nyah dutch""
- Text: ""dh_ah_s_ah_t_n_ah_l_z""
  Note: ""thus utt nulls""
- Text: ""k_ah_p_s_f_l_ah_b_ah_v""
  Note: ""cups flah buv""
- Text: ""sh_ah_f_f_ah_k_sh_ah_z""
  Note: ""shuff fahk shuz""
- Text: ""v_ah_l_dh_q_ah_m_hh_ah_g_z""
  Note: ""vahldh 'um hugs""
- Text: ""s_p_ao_r_t_s_hh_ao_l_r_ao_ng_d""
  Note: ""sports hall wronged""
- Text: ""s_k_ao_r_l_ao_f_ao_r_b_z""
  Note: ""score law forbes""
- Text: ""t_ao_k_t_f_ao_r_d_y_ao_l""
  Note: ""talked ford y'all""
- Text: ""q_ao_r_p_s_n_ao_r_th_d_ao_r_d""
  Note: ""'orps north dord ""
- Text: ""th_ao_t_s_ao_ng_z_g_l_ao_s_t""
  Note: ""thought songs glossed""
- Text: ""w_ao_r_m_th_b_ao_l_g_ao_r""
  Note: ""warmth ball gore""
- Text: ""r_ao_ng_m_ao_n_w_ao_r_n_d""
  Note: ""wrong mawn warned""
- Text: ""b_aw_z_m_aw_dh_z_dh_aw""
  Note: ""bows mouths thou""
- Text: ""t_aw_n_p_r_aw_l_l_aw_d""
  Note: ""town prowl loud""
- Text: ""f_aw_l_k_q_aw_z_th_aw_t""
  Note: ""fawlk 'awz thawt""
- Text: ""n_aw_n_hh_aw_s_d_aw_t""
  Note: ""noun house doubt""
- Text: ""z_ay_g_ay_hh_ay_n_d""
  Note: ""zai guy hind""
- Text: ""q_ay_m_m_ay_t_r_ay_k_s""
  Note: ""'I'm might rikes""
- Text: ""f_ay_s_s_l_ay_w_ay_f""
  Note: ""fice sly wife""
- Text: ""d_ay_d_s_t_ay_l_n_ay_n_th""
  Note: ""died style ninth""
- Text: ""k_r_ay_b_s_k_ay_r_ay_p_s""
  Note: ""cribe sky ripes""
- Text: ""v_ay_v_d_s_ay_n_b_ay_z""
  Note: ""vived sign byes""
- Text: ""p_ay_jh_ay_sh_ay_n""
  Note: ""pie jai shine""
- Text: ""n_eh_n_m_eh_k_g_eh""
  Note: ""nen meck geh""
- Text: ""f_l_eh_sh_v_eh_l_k_eh_r""
  Note: ""flesh vell care""
- Text: ""hh_eh_l_w_eh_l_m_d_q_eh_g_z""
  Note: ""hell whelmed 'eggs""
- Text: ""y_eh_s_s_t_eh_d_b_eh_l_t""
  Note: ""yes stead belt""
- Text: ""p_eh_r_z_sh_eh_t_w_eh_n""
  Note: ""pears shet when""
- Text: ""s_eh_p_t_d_r_eh_m_t_d_eh_v""
  Note: ""sept dreamt dev""
- Text: ""sh_eh_f_s_dh_eh_r_f_eh""
  Note: ""chefs their feh""
- Text: ""p_w_eh_r_r_eh_k_t_ch_eh_r_z""
  Note: ""pwehr rekt chairs""
- Text: ""er_v_dh_er_w_er_th""
  Note: ""erv dher worth""
- Text: ""m_er_dh_er_d_sh_er""
  Note: ""mer dherd sure""
- Text: ""z_er_hh_er_t_s_ch_er_p""
  Note: ""zer hurts chirp""
- Text: ""s_er_d_w_er_l_d_z_v_er_s""
  Note: ""surd worlds verse""
- Text: ""g_er_z_t_q_er_n_d_hh_er_t""
  Note: ""gerzt 'earned hurt""
- Text: ""n_er_v_t_er_y_er_z""
  Note: ""nerve ter yours""
- Text: ""s_l_er_p_y_er_f_er_g""
  Note: ""slurp your ferg""
- Text: ""d_er_z_f_er_m_dh_er_z""
  Note: ""derz ferm dherz""
- Text: ""p_er_k_b_er_n_k_er_b""
  Note: ""perk burn curb""
- Text: ""hh_ey_l_s_r_ey_sh_ey_p""
  Note: ""hail srey shape""
- Text: ""ch_ey_n_jh_d_p_ey_v_d_f_ey_d_z""
  Note: ""changed paved fades""
- Text: ""s_ey_f_w_ey_z_ey_m""
  Note: ""safe ways aim""
- Text: ""ey_dh_ey_y_ey""
  Note: ""ayy they yay""
- Text: ""n_ey_k_s_k_ey_v_d_g_ey_jh""
  Note: ""nakes caved gauge""
- Text: ""p_l_ey_n_z_k_r_ey_hh_ey_v""
  Note: ""plains cray heyv""
- Text: ""f_ey_th_b_ey_k_q_ey_v""
  Note: ""faith bake 'eyv""
- Text: ""m_ey_t_ey_s_d_ey_b""
  Note: ""may tace dabe""
- Text: ""v_ey_g_f_ey_w_ey_v""
  Note: ""vague fay wave""
- Text: ""s_w_ih_f_t_k_ih_l_q_ih_g_z""
  Note: ""swift kill 'iggs""
- Text: ""z_ih_p_m_ih_v_ih_n_s""
  Note: ""zip mih vince""
- Text: ""n_ih_sh_t_ng_ih_ng_w_ih_th""
  Note: ""nished nging with""
- Text: ""p_ih_d_n_ih_k_w_ih_dh""
  Note: ""pid nick with""
- Text: ""d_ih_k_t_dh_ih_s_b_r_ih_m""
  Note: ""dihkt this brim""
- Text: ""th_ih_n_f_ih_r_s_ih_z""
  Note: ""thin fear siz""
- Text: ""s_t_ih_t_l_ih_ng_l_ih_f_t""
  Note: ""stit ling lift""
- Text: ""b_r_ih_ng_dh_ih_n_sh_ih_f_t""
  Note: ""bring dhin shift""
- Text: ""ch_ih_ng_g_ih_r_dh_ih""
  Note: ""ching gear dhih""
- Text: ""y_ih_r_hh_ih_ch_t_jh_ih_g""
  Note: ""year hitched jig""
- Text: ""p_ih_m_p_b_ih_n_r_ih_jh""
  Note: ""pimp bin ridge""
- Text: ""p_iy_w_iy_d_k_iy_m""
  Note: ""pee weed keem""
- Text: ""iy_z_r_iy_m_l_iy_sh""
  Note: ""ease ream leash""
- Text: ""p_l_iy_z_d_m_iy_t_b_iy_d_z""
  Note: ""pleased meat beads""
- Text: ""v_iy_jh_iy_ch_iy_v""
  Note: ""vee gee cheeve""
- Text: ""sh_iy_b_r_iy_dh_d_dh_iy""
  Note: ""she breathed thee""
- Text: ""iy_y_iy_g_iy""
  Note: ""ee yee ghee""
- Text: ""s_iy_k_p_l_iy_f_iy_s_t""
  Note: ""seek plea feast""
- Text: ""z_iy_hh_iy_t_n_iy_th""
  Note: ""zee heat neath""
- Text: ""hh_iy_r_z_s_t_iy_n_k_q_iy_v_z""
  Note: ""heres steenk 'eaves""
- Text: ""b_l_iy_p_d_iy_l_z_w_iy_l""
  Note: ""bleep deals wheel""
- Text: ""g_r_ow_v_m_ow_k_m_ow_s""
  Note: ""grove moke moce""
- Text: ""k_ow_p_dh_ow_b_ow""
  Note: ""cope though bow""
- Text: ""k_l_ow_dh_z_hh_ow_m_d_q_ow_d""
  Note: ""clothes homed 'owed""
- Text: ""t_ow_t_p_ow_w_ow_k""
  Note: ""tote poe woke""
- Text: ""s_ow_y_ow_sh_ow_l""
  Note: ""so yo shole""
- Text: ""ng_ow_hh_ow_f_ow_n""
  Note: ""ngow hoe phone""
- Text: ""d_ow_n_t_g_ow_z_n_ow_z""
  Note: ""don't goes nose""
- Text: ""b_oy_z_jh_oy_n_g_oy""
  Note: ""boys join goy""
- Text: ""p_uh_t_sh_uh_r_w_uh_l_f""
  Note: ""put sure wolf""
- Text: ""g_uh_d_z_y_uh_ng_t_uh_r_z""
  Note: ""goods yuhng tours""
- Text: ""b_uh_sh_k_uh_d_l_uh_k""
  Note: ""bush could look""
- Text: ""f_y_uw_d_uw_m_b_uw_s_t""
  Note: ""few doom boost""
- Text: ""v_y_uw_z_l_uw_k_k_y_uw_b""
  Note: ""views luke cube""
- Text: ""m_uw_dh_m_y_uw_sh_uw_t""
  Note: ""moodh mew shoot""
- Text: ""p_uw_r_g_r_uw_v_r_uw_f""
  Note: ""poor groove roof""
- Text: ""th_r_uw_g_r_uw_y_uw_th""
  Note: ""through grew youth""
- Text: ""f_r_uw_t_q_uw_l_d_t_uw_n""
  Note: ""fruit 'ooled toon""
- Text: ""b_y_uw_w_uw_p_f_uw_l_d""
  Note: ""byoo woop fooled""
- Text: ""s_uw_dh_y_uw_z_n_uw""
  Note: ""soothe use new""
- Text: ""k_uw_hh_uw_m_g_r_uw_p""
  Note: ""koo hoom group""
- Text: ""m_aa_r_k_t_w_aa_n_t_s_t_aa_p""
  Note: ""marked want stop""
- Text: ""k_aa_d_aa_n_t_w_aa_z""
  Note: ""caw daunt wozz""
- Text: ""s_t_aa_r_t_s_g_aa_n_aa_t""
  Note: ""starts gaa not""
- Text: ""w_aa_n_t_s_hh_aa_r_d_d_aa""
  Note: ""wants hard daa""
- Text: ""p_r_aa_n_d_aa_n_t_g_aa_t""
  Note: ""prawn daunt got""
- Text: ""k_ae_n_t_hh_ae_n_d_z_m_ae_p_t""
  Note: ""can't hands mapped""
- Text: ""b_l_ae_s_t_hh_ae_ng_k_dh_ae_t_s""
  Note: ""blast hank that's""
- Text: ""b_ae_n_d_d_ae_n_s_y_ae""
  Note: ""band dance yeah""
- Text: ""g_ae_s_t_k_ae_n_t_t_r_ae_k_s""
  Note: ""gast can't tracks""
- Text: ""dh_ae_t_s_t_ae_n_d_z_l_ae_k_t""
  Note: ""that's tands lacked""
- Text: ""s_t_ae_n_d_dh_ae_t_ae_s""
  Note: ""stand that ass""
- Text: ""jh_ah_s_t_l_ah_v_z_l_ah_sh""
  Note: ""just loves lush""
- Text: ""m_ah_s_t_ah_n_l_ah_v""
  Note: ""must un love""
- Text: ""ah_n_d_s_ah_m_z_ah_n_d""
  Note: ""und some zund""
- Text: ""sh_ah_n_s_t_r_ah_s_t_w_ah_l_z""
  Note: ""shun strust wulls""
- Text: ""k_l_ah_s_dh_ah_k_l_ah_b_z""
  Note: ""cluss the clubs""
- Text: ""b_ah_z_b_l_ah_n_dh_ah_s""
  Note: ""buzz blun thus""
- Text: ""b_ah_n_d_dh_ah_w_ah_t""
  Note: ""bund the what""
- Text: ""l_ah_v_m_ah_n_d_ah_n_t""
  Note: ""love mun dunt""
- Text: ""b_ah_t_s_g_l_ah_n_d_n_ah_t""
  Note: ""butts glund nut""
- Text: ""s_ah_m_b_ah_l_t_ah_ch""
  Note: ""some bahl touch""
- Text: ""t_ah_d_t_ah_l_z_d_r_ah_ng_k""
  Note: ""tud tahlz drunk""
- Text: ""n_ah_s_t_dh_ah_m_dh_ah_s""
  Note: ""nust dhum thus""
- Text: ""n_ah_n_t_s_ah_n_d_l_ah_g""
  Note: ""nunt sund lung""
- Text: ""w_ah_n_t_r_ah_k_s_jh_ah_s_t""
  Note: ""one trucks just""
- Text: ""s_ah_n_t_f_r_ah_m_n_ah_s_t""
  Note: ""sunt from nust""
- Text: ""s_t_ah_f_ah_n_d_y_ah_s""
  Note: ""stuff und yus""
- Text: ""ah_n_w_ah_t_y_ah_ng""
  Note: ""un what young""
- Text: ""s_t_ah_n_d_ah_n_d_w_ah_n_z""
  Note: ""stun dund ones""
- Text: ""k_ah_n_f_r_ah_n_t_b_ah_m_p_t""
  Note: ""kun front bumped""
- Text: ""b_ah_p_ah_n_ah_n_d""
  Note: ""bah pun und""
- Text: ""b_ah_t_s_m_ah_n_th_s_t_ah_m""
  Note: ""butts months tum""
- Text: ""dh_ah_m_w_ah_n_t_ah_b""
  Note: ""dhum one tub""
- Text: ""t_ao_l_m_ao_r_s_ao_l""
  Note: ""tall more soul""
- Text: ""f_ao_r_m_t_ao_r_y_ao_r_k""
  Note: ""form tore york""
- Text: ""y_ao_r_f_ao_r_t_y_ao_r_k""
  Note: ""your fort york""
- Text: ""f_l_ao_r_w_ao_k_s_l_ao_ng""
  Note: ""floor wok slong""
- Text: ""r_aw_n_d_b_aw_t_d_aw_n""
  Note: ""round bout down""
- Text: ""s_ay_d_z_m_ay_n_l_ay_k""
  Note: ""sides mine like""
- Text: ""f_ay_s_t_t_ay_m_z_l_ay_k_s""
  Note: ""feist times likes""
- Text: ""w_ay_n_d_t_r_ay_t_r_ay_d""
  Note: ""wind try tried""
- Text: ""n_ay_t_m_ay_n_d_ay_m_z""
  Note: ""night mine dimes""
- Text: ""f_ay_n_d_m_ay_l_d_t_ay_m""
  Note: ""find mild time""
- Text: ""ay_m_r_ay_t_l_ay_t""
  Note: ""I'm right light""
- Text: ""m_ay_l_ay_f_s_k_l_ay""
  Note: ""my life sklay""
- Text: ""d_r_ay_v_b_l_ay_n_d_f_r_ay_z""
  Note: ""drive blind fries""
- Text: ""k_r_ay_m_ay_n_m_ay_s""
  Note: ""cry mine mice""
- Text: ""f_l_ay_t_s_l_ay_k_t_l_ay_z""
  Note: ""flight sliked lies""
- Text: ""hh_eh_l_p_t_dh_eh_m_s_eh_l_f""
  Note: ""helped them self""
- Text: ""l_eh_f_t_s_eh_s_t_f_r_eh_n_d_z""
  Note: ""left sest friends""
- Text: ""b_eh_s_t_n_eh_v_eh_m""
  Note: ""best nev em""
- Text: ""g_eh_t_dh_eh_r_z_t_eh_r""
  Note: ""get theirs tear""
- Text: ""g_eh_t_s_p_eh_n_t_l_eh_t_s""
  Note: ""get spent lets""
- Text: ""t_eh_l_s_k_eh_r_d_l_eh_k_s""
  Note: ""tell scared lecks""
- Text: ""dh_eh_n_s_eh_d_th_r_eh_d""
  Note: ""then said thread""
- Text: ""f_r_eh_n_d_z_b_r_eh_s_t_s_w_eh_n_t""
  Note: ""friends breasts went""
- Text: ""w_eh_n_y_eh_n_hh_eh_r_z""
  Note: ""when yen hairs""
- Text: ""l_eh_t_m_eh_l_t_s_s_p_eh_k_t""
  Note: ""let melts specked""
- Text: ""v_er_d_t_w_er_l_z_g_er_l_z""
  Note: ""verd twirls girls""
- Text: ""f_er_s_t_k_er_s_t_g_er_l""
  Note: ""first kurst girl""
- Text: ""b_ey_b_g_r_ey_s_t_b_r_ey_k""
  Note: ""babe greyst break""
- Text: ""p_l_ey_s_t_n_ey_m_z_w_ey_t""
  Note: ""placed names wait""
- Text: ""t_r_ey_n_t_p_l_ey_t_s_ey""
  Note: ""traint plate say""
- Text: ""s_t_ey_k_m_ey_d_m_ey_k_s""
  Note: ""steak made makes""
- Text: ""k_r_ey_z_s_t_ey_n_t_ey_s_t_s""
  Note: ""craze stain tastes""
- Text: ""r_ih_t_w_ih_n_d_th_ih_ng_z""
  Note: ""writ wind things""
- Text: ""m_ih_l_k_th_ih_ng_k_ih_t_s""
  Note: ""milk think its""
- Text: ""n_ih_ng_f_ih_t_b_ih_l_z""
  Note: ""ning fit bills""
- Text: ""s_t_ih_l_ih_k_s_p_ih_ng_k""
  Note: ""still icks pink""
- Text: ""s_ih_n_s_p_r_ih_t_g_ih_v""
  Note: ""since prit give""
- Text: ""d_ih_t_s_t_ih_n_d_t_ih_t""
  Note: ""dits tind tit""
- Text: ""w_ih_dh_m_ih_ng_k_s_b_ih_l""
  Note: ""with minks bill""
- Text: ""s_l_ih_t_s_k_ih_n_d_ih_n""
  Note: ""slit skin din""
- Text: ""sh_ih_t_d_r_ih_l_d_ih_ng""
  Note: ""shiht drill ding""
- Text: ""f_l_ih_n_t_k_ih_ng_z_s_ih_ng_k""
  Note: ""flint kings sink""
- Text: ""v_ih_ng_t_r_ih_p_s_b_ih_n""
  Note: ""ving trips bin""
- Text: ""ih_t_hh_ih_z_s_t_ih_ng""
  Note: ""it his sting""
- Text: ""m_ih_n_dh_ih_s_ih_t_s""
  Note: ""min this its""
- Text: ""m_ih_s_t_s_s_l_ih_ng_ih_n_jh""
  Note: ""mists sling inge""
- Text: ""hh_iy_t_w_iy_k_s_k_r_iy_m_z""
  Note: ""heat weak screams""
- Text: ""w_iy_r_s_iy_m_iy_s_t""
  Note: ""we're seem east""
- Text: ""m_iy_n_z_f_r_iy_k_t_m_iy""
  Note: ""means freaked me""
- Text: ""b_iy_t_s_k_iy_p_s_t_r_iy_t_s""
  Note: ""beats keep streets""
- Text: ""d_r_iy_m_s_l_iy_v_m_iy""
  Note: ""dream sleeve me""
- Text: ""sh_iy_l_m_iy_f_iy_l_d_z""
  Note: ""she'll me fields""
- Text: ""f_iy_l_b_iy_z_n_iy_d_z""
  Note: ""feel bees needs""
- Text: ""d_iy_z_w_iy_v_r_iy_l""
  Note: ""deez weave real""
- Text: ""th_r_ow_n_g_ow_s_t_hh_ow_l_d""
  Note: ""thrown ghost hold""
- Text: ""d_ow_n_t_n_ow_t_m_ow_l_d""
  Note: ""don't note mold""
- Text: ""n_ow_n_b_r_ow_k_k_l_ow_s""
  Note: ""known broke close""
- Text: ""b_l_ow_n_w_ow_n_t_m_ow_s_t""
  Note: ""blown won't most""
- Text: ""k_y_uh_r_l_uh_k_s_t_w_uh_d""
  Note: ""cure lookst wood""
- Text: ""y_uw_s_t_uw_b_d_uw_d""
  Note: ""yuce tube dude""
- Text: ""f_y_uw_m_z_y_uw_t_uw_d""
  Note: ""fumes you tude""
- Text: ""t_r_uw_th_k_y_uw_t_t_uw_m""
  Note: ""truth cute toom""
- Text: ""p_y_uw_g_y_uw_y_uw_th""
  Note: ""pyoo gyoo youth""
- Text: ""m_uw_v_t_uw_s_k_uw_l""
  Note: ""move to school""
- Text: ""t_uw_l_y_uw_z_d_t_uw_l_z""
  Note: ""tool used tools""
- Text: ""g_y_uw_n_uw_n_y_uw_m""
  Note: ""gyoo new nyoom""
- Text: ""f_uw_l_d_y_uw_k_y_uw""
  Note: ""fooled you cue""";

        public Settings() { }

        public Settings(bool startup)
        {
            Bass.Init();
            Bass.RecordInit();
            recList = new ObservableCollection<RecListItem>();
            LoadDefault();
            init = true;
        }

        public void LoadDefault()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var defaultSettings = Path.Combine(currentDirectory, "default.arp");

            readUnicode = false; //TEMPORARY
            splitWhitespace = true; //TEMPORARY

            if (File.Exists(defaultSettings))
            {
                LoadSettings(defaultSettings);
            }
            else
            {
                DestinationFolder = Path.Combine(currentDirectory, "voicebank");

                AudioDriver = Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
                AudioInputDevice = Bass.CurrentRecordingDevice;
                AudioInputLevel = 100;
                AudioOutputDevice = Bass.CurrentDevice;
                AudioOutputLevel = 100;

                FontSize = 24;

                var deserializer = new Deserializer();
                var defaultRecList = deserializer.Deserialize<ObservableCollection<RecListItem>>(defaultRecListRaw);
                foreach (RecListItem item in defaultRecList)
                {
                    item.CreateAudio(this);
                    RecList.Add(item);
                }

                SaveSettings(defaultSettings);
            }

            ProjectFile = "";
            recListFile = "List loaded from default.";
        }

        private string recListFile;
        [YamlIgnore]
        public string RecListFile
        {
            get
            {
                return recListFile;
            }
            set
            {
                recListFile = value;
                LoadRecList();
            }
        }

        private bool readUnicode;
        [YamlIgnore]
        public bool ReadUnicode
        {
            get { return readUnicode; }
            set
            {
                readUnicode = value;
                LoadRecList();
            }
        }

        private bool splitWhitespace;
        [YamlIgnore]
        public bool SplitWhitespace
        {
            get { return splitWhitespace; }
            set
            {
                splitWhitespace = value;
                LoadRecList();
            }
        }

        private ObservableCollection<RecListItem> recList;
        public ObservableCollection<RecListItem> RecList
        {
            get { return recList; }
            set { recList = value; }
        }
        public void LoadRecList()
        {
            if (init)
            {
                //recList = new ObservableCollection<RecListItem>();
                HashSet<string> uniqueStrings = new HashSet<string>();

                Encoding e;
                if (ReadUnicode)
                {
                    e = Encoding.UTF8;
                }
                else
                {
                    e = CodePagesEncodingProvider.Instance.GetEncoding(932);
                }

                var ext = Path.GetExtension(RecListFile);
                if (ext == ".txt")
                {
                    string[] textArr;
                    if (SplitWhitespace)
                    {
                        var rawText = File.ReadAllText(RecListFile, e);
                        rawText = Regex.Replace(rawText, @"\s{2,}", " ");
                        textArr = Regex.Split(rawText, @"\s");
                    }
                    else
                    {
                        textArr = File.ReadAllLines(RecListFile, e);
                    }

                    foreach (string line in textArr)
                    {
                        if (!uniqueStrings.Contains(line))
                        {
                            RecList.Add(new RecListItem(this, line));
                            uniqueStrings.Add(line);
                        }
                    }
                }
                else if (ext == ".arl")
                {
                    var rawText = File.ReadAllText(RecListFile);
                    var deserializer = new YamlDotNet.Serialization.Deserializer();
                    var tempDict = deserializer.Deserialize<Dictionary<string, string>>(rawText);
                    foreach (var item in tempDict)
                    {
                        RecList.Add(new RecListItem(this, item.Key, item.Value));
                    }
                }
            }
        }

        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }

        [YamlIgnore]
        public List<string> AudioInputDeviceList
        {
            get
            {
                var temp = new List<string>();
                if (init)
                {
                    for (var i = 0; i < Bass.RecordingDeviceCount; i++)
                    {
                        temp.Add(Bass.RecordGetDeviceInfo(i).Name);
                    }
                }
                return temp;
            }
        }

        public int AudioInputDevice { get; set; }

        private int _audioInputLevel;
        public int AudioInputLevel
        {
            get { return _audioInputLevel; }
            set
            {
                if (value >= 0 && value <= 100)
                    _audioInputLevel = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        [YamlIgnore]
        public List<string> AudioOutputDeviceList
        {
            get
            {
                var temp = new List<string>();
                if (init)
                {
                    for (var i = 0; i < Bass.DeviceCount; i++)
                    {
                        temp.Add(Bass.GetDeviceInfo(i).Name);
                    }
                }
                return temp;
            }
        }

        public int AudioOutputDevice { get; set; }

        private int _audioOutputLevel;
        public int AudioOutputLevel 
        { 
            get { return _audioOutputLevel; } 
            set
            {
                if (value >= 0 && value <= 100)
                    _audioOutputLevel = value;
                else
                    throw new ArgumentOutOfRangeException();
            } 
        }

        private int fontSize;
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (value >= 8 && value <= 200)
                    fontSize = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        [YamlIgnore]
        public string ProjectFile { get; set; }

        public void LoadSettings(string path)
        {
            ProjectFile = path;

            var raw = File.ReadAllText(path);
            var deserializer = new Deserializer();
            var newSettings = deserializer.Deserialize<Settings>(raw);

            recListFile = "List loaded from project file.";
            DestinationFolder = newSettings.DestinationFolder;

            AudioDriver = newSettings.AudioDriver;
            AudioInputDevice = newSettings.AudioInputDevice;
            AudioInputLevel = newSettings.AudioInputLevel;
            AudioOutputDevice = newSettings.AudioOutputDevice;
            AudioOutputLevel = newSettings.AudioOutputLevel;

            FontSize = newSettings.FontSize;

            recList.Clear();
            foreach (RecListItem item in newSettings.RecList)
            {
                item.CreateAudio(this);
                RecList.Add(item);
            }
        }

        public void SaveSettings(string path)
        {
            using (StreamWriter file = new(path))
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(file, this);
            }
        }
    }
}
