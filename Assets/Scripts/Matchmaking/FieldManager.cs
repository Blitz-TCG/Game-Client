using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public List<AllField> fieldsLimit = new();
    public int CloneCounter = 0;
    public int MeteorCounter = 0;
    public int EvolveCounter = 0;
    public int MalignantCounter = 0;
    public int GoodFavorCounter = 0;
    public int SummonCounter = 0;
    public int SerenityCounter = 0;
    public int MutateCounter = 0;
    public int RenewalCounter = 0;
    public int GoadCounter = 0;
    public int KamikazeCounter = 0;
    public int BerserkerCounter = 0;
    public int CritCounter = 0;
    public int HungerCounter = 0;
    public int ScattershotCounter = 0;
    public int FarmerCounter = 0;
    public int BusterCounter = 0;
    public int MasonCounter = 0;
    public int ParalyzeCounter = 0;
    public int CurseCounter = 0;
    public int DoomCounter = 0;
    public int GambitCounter = 0;
    public int SilenceCounter = 0;
    public int StealthCounter = 0;
    public int SmiteCounter = 0;
    public int SacrificeCounter = 0;
    public int MimicCounter = 0;
    public int GeneralBaneCounter = 0;
    public int BlackholeCounter = 0;
    public int NuclearCounter = 0;
    public int EndgameCounter = 0;
    public int FodderCounter = 0;
    public int RepairCounter = 0;
    public int GeneralBoonCounter = 0;
    public int WhiteFlagCounter = 0;
    public int EclipseCounter = 0;
    public int GeneralAegisCounter = 0;
    public int FearCounter = 0;
    public int ToxicCounter = 0;
    public int ShhhCounter = 0;
    public int SwapCounter = 0;
    public int HexCounter = 0;
    public int BanCounter = 0;
    public int ConsumeCounter = 0;
    public int MergeCounter = 0;
    public int DrainCounter = 0;
    public int WarcryCounter = 0;
    public int StackedOddsCounter = 0;
    public int CowardCounter = 0;
    public int IllusionCounter = 0;
    public int HecklerCounter = 0;
    public int OvertimeCounter = 0;
    public int AssassinCounter = 0;
    public int BlitzCounter = 0;
    public int ExecutionerCounter = 0;
    public int LimitlessCounter = 0;
    public int ShieldCounter = 0;
    public int RallyCounter = 0;
    public int FeastCounter = 0;
    public int TruesightCounter = 0;
    public int HeathenCounter = 0;
    public int EvasiveCounter = 0;
    public int FleetFootedCounter = 0;
    public int StifleCounter = 0;
    public int TimecrunchCounter = 0;
    public int TaxesCounter = 0;
    public int RageCounter = 0;
    public int DuelCounter = 0;
    public int GeneralWardCounter = 0;
    public int SubsidyCounter = 0;
    public int StalemateCounter = 0;
    public int MorphCounter = 0;
    public int TankCounter = 0;
    public int GuardianCounter = 0;
    public int RespiteCounter = 0;
    public int SavingsCounter = 0;
    public int FlourishCounter = 0;
    public int NoneCounter = 0;
    public int DeActivateCounter = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CalculateAbilityCounter(GameObject playerField)
    {
        for (int i = 0; i < playerField.transform.childCount; i++)
        {
            Debug.Log(playerField.transform.childCount + " playerField.transform.childCount ");
            if (playerField.transform.GetChild(i).childCount > 0)
            {
                Debug.Log(playerField.transform.GetChild(i).childCount + " playerField.transform.GetChild(i).childCount ");
                GameObject playerCard = playerField.transform.GetChild(i).GetChild(0).GetChild(0).gameObject;
                Debug.Log(playerCard + " player card");
                if (playerCard.GetComponent<Clone>())
                {
                    CloneCounter++;
                }
                else if (playerCard.GetComponent<Meteor>())
                {
                    MeteorCounter++;
                }
                else if (playerCard.GetComponent<Evolve>())
                {
                    EvolveCounter++;
                }
                else if (playerCard.GetComponent<Malignant>())
                {
                    MalignantCounter++;
                }
                else if (playerCard.GetComponent<GoodFavor>())
                {
                    GoodFavorCounter++;
                }
                else if (playerCard.GetComponent<Summon>())
                {
                    SummonCounter++;
                }
                else if (playerCard.GetComponent<Serenity>())
                {
                    SerenityCounter++;
                }
                else if (playerCard.GetComponent<Mutate>())
                {
                    MutateCounter++;
                }
                else if (playerCard.GetComponent<Renewal>())
                {
                    RenewalCounter++;
                }
                else if (playerCard.GetComponent<Goad>())
                {
                    GoadCounter++;
                }
                else if (playerCard.GetComponent<Kamikaze>())
                {
                    KamikazeCounter++;
                }
                else if (playerCard.GetComponent<Berserker>())
                {
                    BerserkerCounter++;
                }
                else if (playerCard.GetComponent<Crit>())
                {
                    CritCounter++;
                }
                else if (playerCard.GetComponent<Hunger>())
                {
                    HungerCounter++;
                }
                else if (playerCard.GetComponent<Scattershot>())
                {
                    ScattershotCounter++;
                }
                else if (playerCard.GetComponent<Farmer>())
                {
                    FarmerCounter++;
                }
                else if (playerCard.GetComponent<Buster>())
                {
                    BusterCounter++;
                }
                else if (playerCard.GetComponent<Mason>())
                {
                    MasonCounter++;
                }
                else if (playerCard.GetComponent<Paralyze>())
                {
                    ParalyzeCounter++;
                }
                else if (playerCard.GetComponent<Curse>())
                {
                    CurseCounter++;
                }
                else if (playerCard.GetComponent<Doom>())
                {
                    DoomCounter++;
                }
                else if (playerCard.GetComponent<Gambit>())
                {
                    GambitCounter++;
                }
                else if (playerCard.GetComponent<Smite>())
                {
                    SmiteCounter++;
                }
                else if (playerCard.GetComponent<GeneralBane>())
                {
                    GeneralBaneCounter++;
                }
                else if (playerCard.GetComponent<Blackhole>())
                {
                    BlackholeCounter++;
                }
                else if (playerCard.GetComponent<Nuclear>())
                {
                    NuclearCounter++;
                }
                else if (playerCard.GetComponent<Repair>())
                {
                    RepairCounter++;
                }
                else if (playerCard.GetComponent<GeneralBoon>())
                {
                    GeneralBoonCounter++;
                }
                //else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}else if (playerCard.GetComponent<Meteor>())
                //{
                //    MeteorCounter++;
                //}
            }
        }
        Debug.Log(CloneCounter + " CloneCounter");
        Debug.Log(MeteorCounter + " MeteorCounter");
        Debug.Log(EvolveCounter + " EvolveCounter");
        Debug.Log(MalignantCounter + " MalignantCounter");
        Debug.Log(GoodFavorCounter + " GoodFavorCounter");
        Debug.Log(SummonCounter + " SummonCounter");
        Debug.Log(SerenityCounter + " SerenityCounter");
        Debug.Log(MutateCounter + " MutateCounter");
        Debug.Log(RenewalCounter + " RenewalCounter");
        Debug.Log(GoadCounter + " GoadCounter");
        Debug.Log(KamikazeCounter + " KamikazeCounter");
        Debug.Log(BerserkerCounter + " BerserkerCounter");
        Debug.Log(CritCounter + " CritCounter");
        Debug.Log(HungerCounter + " HungerCounter");
        Debug.Log(ScattershotCounter + " ScattershotCounter");
        Debug.Log(FarmerCounter + " FarmerCounter");
        Debug.Log(BusterCounter + " BusterCounter");
        Debug.Log(MasonCounter + " MasonCounter");
        Debug.Log(ParalyzeCounter + " ParalyzeCounter");
        Debug.Log(CurseCounter + " CurseCounter");
        Debug.Log(DoomCounter + " DoomCounter");
        Debug.Log(GambitCounter + " GambitCounter");
        Debug.Log(SmiteCounter + " SmiteCounter");
        Debug.Log(GeneralBaneCounter + " GeneralBaneCounter");
        Debug.Log(BlackholeCounter + " BlackholeCounter");
        Debug.Log(NuclearCounter + " NuclearCounter");
        Debug.Log(RepairCounter + " RepairCounter");
        Debug.Log(GeneralBoonCounter + " GeneralBoonCounter");
        Debug.Log(EndgameCounter + " EndgameCounter");
    }

    public int GetAbilityCounter(CardAbility cardAbility)
    {

        Debug.Log(cardAbility + " ability card");

        switch (cardAbility)
        {
            case CardAbility.Clone: return CloneCounter;
            case CardAbility.Meteor: return MeteorCounter;
            case CardAbility.Evolve: return EvolveCounter;
            case CardAbility.Malignant: return MalignantCounter;
            case CardAbility.GoodFavor: return GoodFavorCounter;
            case CardAbility.Summon: return SummonCounter;
            case CardAbility.Serenity: return SerenityCounter;
            case CardAbility.Mutate: return MutateCounter;
            case CardAbility.Renewal: return RenewalCounter;
            case CardAbility.Goad: return GoadCounter;
            case CardAbility.Kamikaze: return KamikazeCounter;
            case CardAbility.Berserker: return BerserkerCounter;
            case CardAbility.Crit: return CritCounter;
            case CardAbility.Hunger: return HungerCounter;
            case CardAbility.Scattershot: return ScattershotCounter;
            case CardAbility.Farmer: return FarmerCounter;
            case CardAbility.Buster: return BusterCounter;
            case CardAbility.Mason: return MasonCounter;
            case CardAbility.Paralyze: return ParalyzeCounter;
            case CardAbility.Curse: return CurseCounter;
            case CardAbility.Doom: return DoomCounter;
            case CardAbility.Gambit: return GambitCounter;
            case CardAbility.Smite: return SmiteCounter;
            case CardAbility.GeneralBane: return GeneralBaneCounter;
            case CardAbility.Nuclear: return NuclearCounter;
            case CardAbility.Repair: return RepairCounter;
            case CardAbility.GeneralBoon: return GeneralBoonCounter;
            case CardAbility.None: return NoneCounter;
            case CardAbility.DeActivate: return DeActivateCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            //case CardAbility.Clone:
            //    CloneCounter++;
            //    return CloneCounter;
            default:
                return 0; 
        }
    }

    public void ResetCounters()
    {
        CloneCounter = 0;
        MeteorCounter = 0;
        EvolveCounter = 0;
        MalignantCounter = 0;
        GoodFavorCounter = 0;
        SummonCounter = 0;
        SerenityCounter = 0;
        MutateCounter = 0;
        RenewalCounter = 0;
        GoadCounter = 0;
        KamikazeCounter = 0;
        BerserkerCounter = 0;
        CritCounter = 0;
        HungerCounter = 0;
        ScattershotCounter = 0;
        FarmerCounter = 0;
        BusterCounter = 0;
        MasonCounter = 0;
        ParalyzeCounter = 0;
        CurseCounter = 0;
        DoomCounter = 0;
        GambitCounter = 0;
        SilenceCounter = 0;
        StealthCounter = 0;
        SmiteCounter = 0;
        SacrificeCounter = 0;
        MimicCounter = 0;
        GeneralBaneCounter = 0;
        BlackholeCounter = 0;
        NuclearCounter = 0;
        EndgameCounter = 0;
        FodderCounter = 0;
        RepairCounter = 0;
        GeneralBoonCounter = 0;
        WhiteFlagCounter = 0;
        EclipseCounter = 0;
        GeneralAegisCounter = 0;
        FearCounter = 0;
        ToxicCounter = 0;
        ShhhCounter = 0;
        SwapCounter = 0;
        HexCounter = 0;
        BanCounter = 0;
        ConsumeCounter = 0;
        MergeCounter = 0;
        DrainCounter = 0;
        WarcryCounter = 0;
        StackedOddsCounter = 0;
        CowardCounter = 0;
        IllusionCounter = 0;
        HecklerCounter = 0;
        OvertimeCounter = 0;
        AssassinCounter = 0;
        BlitzCounter = 0;
        ExecutionerCounter = 0;
        LimitlessCounter = 0;
        ShieldCounter = 0;
        RallyCounter = 0;
        FeastCounter = 0;
        TruesightCounter = 0;
        HeathenCounter = 0;
        EvasiveCounter = 0;
        FleetFootedCounter = 0;
        StifleCounter = 0;
        TimecrunchCounter = 0;
        TaxesCounter = 0;
        RageCounter = 0;
        DuelCounter = 0;
        GeneralWardCounter = 0;
        SubsidyCounter = 0;
        StalemateCounter = 0;
        MorphCounter = 0;
        TankCounter = 0;
        GuardianCounter = 0;
        RespiteCounter = 0;
        SavingsCounter = 0;
        FlourishCounter = 0;
        NoneCounter = 0;
        DeActivateCounter = 0;
    }
}