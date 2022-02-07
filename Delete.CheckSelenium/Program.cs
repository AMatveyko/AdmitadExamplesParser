using System;
using System.Collections.Generic;
using SearchEngineIndexChecking.Workers;

namespace Delete.CheckSelenium
{
    class Program
    {
        static void Main(string[] args) {
            
                        var urls2 = new List<string>() {
                "https://thestore.ru/dlya/zhenschin/color-aivori/",
                "https://thestore.ru/dlya/zhenschin/brand-wisell/",
                "https://thestore.ru/dlya/zhenschin/color-liloviy/",
                "https://thestore.ru/dlya/zhenschin/shop-respectshoes/",
                "https://thestore.ru/dlya/zhenschin/brand-seed/",
                "https://thestore.ru/dlya/zhenschin/brand-intimissimi/",
                "https://thestore.ru/dlya/zhenschin/shop-reebok/",
                "https://thestore.ru/dlya/zhenschin/brand-pilyq/",
                "https://thestore.ru/dlya/zhenschin/brand-keepsake/",
                "https://thestore.ru/dlya/zhenschin/brand-catisa/",
                "https://thestore.ru/dlya/zhenschin/brand-hermitage/",
                "https://thestore.ru/dlya/zhenschin/brand-altea/",
                "https://thestore.ru/dlya/zhenschin/color-slivoviy/",
                "https://thestore.ru/dlya/zhenschin/shop-birkenstock/",
                "https://thestore.ru/dlya/zhenschin/shop-marksandspencer/",
                "https://thestore.ru/dlya/zhenschin/color-lazurniy/",
                "https://thestore.ru/dlya/zhenschin/sostav-poliuretan/",
                "https://thestore.ru/dlya/zhenschin/sostav-hrustal/",
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_serebristye/",
                "https://thestore.ru/dlya/zhenschin/shop-newbalance/",
                "https://thestore.ru/dlya/zhenschin/brand-memjs/",
                "https://thestore.ru/dlya/zhenschin/shop-beru/",
                "https://thestore.ru/dlya/zhenschin/brand-milly/",
                "https://thestore.ru/dlya/zhenschin/color-terrakotoviy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/koshelki/kupit_na_vysokoi_platforme/",
                "https://thestore.ru/dlya/zhenschin/brand-roobins/",
                "https://thestore.ru/dlya/zhenschin/size-56/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-sarman/",
                "https://thestore.ru/dlya/zhenschin/belyo/brand-valentina/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-tigerlily/",
                "https://thestore.ru/dlya/zhenschin/brand-manas/",
                "https://thestore.ru/dlya/zhenschin/size-140/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-noemi/",
                "https://thestore.ru/dlya/zhenschin/belyo/sorochki_neglizhe/kupit_dlya_leta/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kolca/kupit_obruchalnye/",
                "https://thestore.ru/dlya/zhenschin/brand-tagliatore/",
                "https://thestore.ru/dlya/zhenschin/sostav-tekstil/",
                "https://thestore.ru/dlya/zhenschin/sostav-denim/",
                "https://thestore.ru/dlya/zhenschin/brand-seventy/",
                "https://thestore.ru/dlya/zhenschin/shop-tamaris/",
                "https://thestore.ru/dlya/zhenschin/brand-altea/",
                "https://thestore.ru/dlya/zhenschin/size-140/",
                "https://thestore.ru/dlya/zhenschin/brand-majorelle/",
                "https://thestore.ru/dlya/zhenschin/color-aivori/",
                "https://thestore.ru/dlya/zhenschin/krasota/kupit_dorogo/",
                "https://thestore.ru/dlya/zhenschin/odezhda/",
                "https://thestore.ru/dlya/zhenschin/shop-svmoscow/",
                "https://thestore.ru/dlya/zhenschin/brand-moncler/",
                "https://thestore.ru/dlya/zhenschin/sostav-tvid/",
                "https://thestore.ru/dlya/zhenschin/brand-milly/"
            };
            
            var results = IndexChecker.CheckUrls(urls2, 5);
        }
    }
}