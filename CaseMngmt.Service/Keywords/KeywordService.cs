﻿using AutoMapper;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Repository.Keywords;

namespace CaseMngmt.Service.Keywords
{
    public class KeywordService : IKeywordService
    {
        private IKeywordRepository _repository;
        private readonly IMapper _mapper;
        public KeywordService(IKeywordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(KeywordRequest request)
        {
            try
            {
                var entity = _mapper.Map<Keyword>(request);
                return await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<KeywordViewModel>?> GetAllAsync(int pageSize, int pageNumber)
        {
            try
            {
                var result = await _repository.GetAllAsync(pageSize, pageNumber);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<KeywordViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                var result = _mapper.Map<KeywordViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Guid Id, KeywordRequest request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(Id);
                if (entity == null)
                {
                    return 0;
                }

                entity.Name = request.Name;
                entity.MaxLength = request.MaxLength;
                entity.IsRequired = request.IsRequired;
                entity.Order = request.Order;
                entity.CaseSearchable = request.CaseSearchable;
                entity.DocumentSearchable = request.DocumentSearchable;
                entity.UpdatedDate = DateTime.UtcNow;
                await _repository.UpdateAsync(entity);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
