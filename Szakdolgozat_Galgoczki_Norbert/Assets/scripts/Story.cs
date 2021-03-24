using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Story : MonoBehaviour
{   
    private string[] tortenet;
    private Text text;
    private int index=0;
    private int szoveghossz=4;
    private float delay=0;
    private float maxdelay=4;
    // Start is called before the first frame update
   
    void Start()
    {   //wilág Faerûn
        //elf,mage,name: Cat lady -> Cat
        tortenet = new  string[]{"Egy elfeneledett világban, Faerûn északnyugati vidékén",
                                "Élt egy Elf varázslólány ,akit csak úgy ismertek, hogy 'CatLady'",
                                "akinenek akkora varázs ereje volt hogy csak hobortből átformálta",
                                "a kontinens egyes részeit, áll róla és egykori csapatáró a világ ",
                                "leghatalmasabb szobrai a kontinens legnagyobb és legszebb városába",
                                "amit egyesek csak úgy ismernek hogy a 'Pompa város' vagy az 'észak koronája'",
                                "Ez a város nem más mint Waterdeep meg annyi rejtély és kincs városa.",
                                "Miután CatLady és a csapata elváltak,minden napja egyre unalmasabbá vált",
                                "Nem boldogitotta a megszámlálhatatlan mennyiségü aranya melyet a kalandjai során",
                                "szerzet, senem a kapalndok amelyekbe részvesz.",
                                "Egynap úgy határozott hogy megoldást fog találni hogy újra boldognak",
                                "érezhesse magát. Elhatározta hogy Létrehozza a világ leghatalmasabb Kazamtáját",
                                "melyben egy régi modi labirintust helyez el és minden héten a világ",
                                "leghatalmasabb varázslatát arra használja majd hogy egy értelmes",
                                "élőlényt véletlenszerüen ideiglenesen a Kazamatájába teleportáljon és elzárja",
                                "erre az időre az alany összes varázserejét, hogy azon tudjon szorakozni",
                                "ahogy probál kijutni a játékos, de mivel CatLady egy nagyon jó szivű elf",
                                "ezért amikor az alany eléri a Kazamata végét vagy megsérűlne akkor",
                                "vissza kerűl oda ahol volt épségben és 20 arannyal a zsebében",
                                "A játékunk ennek a Kazamatának a Labirintusában fog játszodni",
                                "ahol mi vagyunk a szerencsés(vagy szerencsétlen) lények akik bekerűlteka",
                                "a Kazamatába és probálnak kijutni"
                                };
        text = GameObject.Find("UI_story").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {  
        delay -= Time.deltaTime;
        if(delay<=0){
            Debug.Log(index);
            delay=maxdelay;
            string vegeredmeny="";
            for(int i =0;i<szoveghossz;i++){
                if(index+i<tortenet.Length)
                vegeredmeny+=tortenet[index+i]+"\n";
            }
            index++;
            text.text=vegeredmeny;
        }
        if(index==tortenet.Length){
            SceneManager.LoadScene(0);
        }
        if (Input.GetMouseButtonDown(0))
            delay=0;
    }
}
