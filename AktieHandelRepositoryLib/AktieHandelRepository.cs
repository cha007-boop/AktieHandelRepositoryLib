using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public class AktieHandelRepository : IAktieHandelRepository
    {
        #region Instance fields
        private List<AktieHandel> _aktieHandelList;
        private static int _nextId = 1; // Static field to keep track of the next ID
        #endregion

        #region Constructors
        public AktieHandelRepository()
        {
            _aktieHandelList = new List<AktieHandel>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieves an <see cref="AktieHandel"/> object by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the <see cref="AktieHandel"/> to retrieve.</param>
        /// <returns>The <see cref="AktieHandel"/> object if found; otherwise, <see langword="null"/>.</returns>
        public AktieHandel? GetById(int id)
        {
            return _aktieHandelList.Find(aktie => aktie.Id == id);
        }

        /// <summary>
        /// Retrieves all <see cref="AktieHandel"/> objects from the repository.
        /// </summary>
        /// <returns>A list of all <see cref="AktieHandel"/> objects.</returns>
        public List<AktieHandel> GetAll()
        {
            return _aktieHandelList;
        }

        public List<AktieHandel> Get()
        {
            return _aktieHandelList;
        }

        /// <summary>
        /// Adds a new <see cref="AktieHandel"/> to the repository.
        /// </summary>
        /// <param name="aktieHandel">The <see cref="AktieHandel"/> to add.</param>
        /// <returns>The added <see cref="AktieHandel"/> object.</returns>
        public AktieHandel Add(AktieHandel aktieHandel)
        {
            if (aktieHandel == null)
            {
                throw new ArgumentNullException(nameof(aktieHandel), "AktieHandel cannot be null.");
            }
            // Assign a unique ID to the new AktieHandel
            aktieHandel = new AktieHandel(_nextId++, aktieHandel.Name, aktieHandel.Amount, aktieHandel.ExchangePrice);
            _aktieHandelList.Add(aktieHandel);
            return aktieHandel;
        }

        /// <summary>
        /// Deletes an <see cref="AktieHandel"/> from the repository based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="AktieHandel"/> to delete.</param>
        /// <returns>The deleted <see cref="AktieHandel"/> object if found; otherwise, <see langword="null"/>.</returns>
        public AktieHandel? Delete(int id)
        {
            var aktieHandel = GetById(id);
            if (aktieHandel != null)
            {
                _aktieHandelList.Remove(aktieHandel);
                return aktieHandel;
            }
            return null;
        }


        /// <summary>
        /// Updates an existing <see cref="AktieHandel"/> in the repository based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="AktieHandel"/> to update.</param>
        /// <param name="aktie">The updated <see cref="AktieHandel"/> object.</param>
        /// <returns>The updated <see cref="AktieHandel"/> object if found; otherwise, <see langword="null"/>.</returns>
        public AktieHandel? Update(int id, AktieHandel aktie)
        {
            var existingAktie = GetById(id);
            if (existingAktie != null)
            {
                existingAktie.Name = aktie.Name;
                existingAktie.Amount = aktie.Amount;
                existingAktie.ExchangePrice = aktie.ExchangePrice;
            }
            return existingAktie;
        }
        #endregion
    }
}
