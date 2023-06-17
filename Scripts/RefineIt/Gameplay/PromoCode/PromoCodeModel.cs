using System;
using Gameplay.Currencies;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.PromoCode
{
    public class PromoCodeModel
    {
        public event Action<PromoCodeStatusType> PromoCodeStatus;

        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly CurrenciesModel _currenciesModel;

        public PromoCodeModel(IStaticDataService staticDataService, IProgressService progressService,
            CurrenciesModel currenciesModel)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
        }

        public void Initialize()
        {
            foreach (var promoCodeData in _staticDataService.PromoCodesConfig.PromoCodesData)
            {
                var promoCodeProgress = _progressService.PlayerProgress.PromoCodeProgresses
                    .Find(x => x.Key == promoCodeData.Key);

                if (promoCodeProgress == null)
                {
                    _progressService.PlayerProgress.CreatePromoCodeProgress(promoCodeData.Key,
                        promoCodeData.FromDateTime, promoCodeData.ToTheTime);
                }
            }
        }

        public void EnterPromoCode(string key)
        {
            var statusType = CheckAvailablePromoCode(key, out var promoCodeData,
                out var promoCodeProgress);

            if (statusType == PromoCodeStatusType.Available)
            {
                foreach (var currency in promoCodeData.Currency)
                {
                    _currenciesModel.Add(currency);
                }

                promoCodeProgress.IsEntered = true;
            }

            PromoCodeStatus?.Invoke(statusType);
        }

        private PromoCodeStatusType CheckAvailablePromoCode(string key, out PromoCodeData promoCodeData,
            out PromoCodeProgress promoCodeProgress)
        {
            promoCodeProgress = _progressService.PlayerProgress.PromoCodeProgresses
                .Find(x => x.Key == key);

            promoCodeData = _staticDataService.PromoCodesConfig.PromoCodesData
                .Find(x => x.Key == key);

            if (promoCodeProgress == null || promoCodeData == null)
            {
                return PromoCodeStatusType.Failed;
            }

            if (promoCodeProgress.isExpired)
            {
                return PromoCodeStatusType.Expired;
            }
            
            if (promoCodeProgress.IsEntered)
            {
                return PromoCodeStatusType.Entered;
            }

            var toTheTimePromoCode = DateTime.Parse(promoCodeProgress.ToTheTime);

            if (DateTime.Now > toTheTimePromoCode)
            {
                promoCodeProgress.isExpired = true;
                return PromoCodeStatusType.Expired;
            }

            return PromoCodeStatusType.Available;
        }
    }
}