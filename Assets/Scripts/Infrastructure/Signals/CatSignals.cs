using Core.Cats;

namespace Core.Infrastructure.Signals.Cats
{
    public struct CatFellSignal
    {
        public CatView FallenCat;
    }
    public struct CatSavedSignal 
    {
        public CatView SavedCat;
    }
    public struct CatKidnappedSignal
    {
        public CatView KidnappedCat;
    }
}
