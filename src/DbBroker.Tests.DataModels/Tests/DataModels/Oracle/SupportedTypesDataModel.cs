using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oracle.ManagedDataAccess.Client;
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;
using System.Collections.Generic;

namespace DbBroker.Tests.DataModels.Oracle
{
    [Table("SUPPORTED_TYPES", Schema = "DBBROKER")]
    public class SupportedTypesDataModel : DataModel<SupportedTypesDataModel>
    {
    
        private string? _Varchar2TypeMax;

        [Column("VARCHAR2_TYPE_MAX"), ColumnType(OracleDbType.Varchar2)]
        public string? Varchar2TypeMax
        { 
            get
            {
                return _Varchar2TypeMax;
            } 
            set
            {
                _IsNotPristine[nameof(Varchar2TypeMax)] = true;
                _Varchar2TypeMax = value;
            }
        }

        private string? _Varchar2TypeMin;

        [Column("VARCHAR2_TYPE_MIN"), ColumnType(OracleDbType.Varchar2)]
        public string? Varchar2TypeMin
        { 
            get
            {
                return _Varchar2TypeMin;
            } 
            set
            {
                _IsNotPristine[nameof(Varchar2TypeMin)] = true;
                _Varchar2TypeMin = value;
            }
        }

        private DateTime _DateType;

        [Column("DATE_TYPE"), ColumnType(OracleDbType.Date)]
        public DateTime DateType
        { 
            get
            {
                return _DateType;
            } 
            set
            {
                _IsNotPristine[nameof(DateType)] = true;
                _DateType = value;
            }
        }

        private DateTime _TimestampType;

        [Column("TIMESTAMP_TYPE"), ColumnType(OracleDbType.Date)]
        public DateTime TimestampType
        { 
            get
            {
                return _TimestampType;
            } 
            set
            {
                _IsNotPristine[nameof(TimestampType)] = true;
                _TimestampType = value;
            }
        }

        private decimal _NumberType;

        [Column("NUMBER_TYPE"), ColumnType(OracleDbType.Decimal)]
        public decimal NumberType
        { 
            get
            {
                return _NumberType;
            } 
            set
            {
                _IsNotPristine[nameof(NumberType)] = true;
                _NumberType = value;
            }
        }

        private decimal _BooleanType;

        [Column("BOOLEAN_TYPE"), ColumnType(OracleDbType.Decimal)]
        public decimal BooleanType
        { 
            get
            {
                return _BooleanType;
            } 
            set
            {
                _IsNotPristine[nameof(BooleanType)] = true;
                _BooleanType = value;
            }
        }

        private byte[]? _BlobType;

        [Column("BLOB_TYPE"), ColumnType(OracleDbType.Blob)]
        public byte[]? BlobType
        { 
            get
            {
                return _BlobType;
            } 
            set
            {
                _IsNotPristine[nameof(BlobType)] = true;
                _BlobType = value;
            }
        }

        private string? _ClobType;

        [Column("CLOB_TYPE"), ColumnType(OracleDbType.Clob)]
        public string? ClobType
        { 
            get
            {
                return _ClobType;
            } 
            set
            {
                _IsNotPristine[nameof(ClobType)] = true;
                _ClobType = value;
            }
        }

        private string? _NclobType;

        [Column("NCLOB_TYPE"), ColumnType(OracleDbType.NClob)]
        public string? NclobType
        { 
            get
            {
                return _NclobType;
            } 
            set
            {
                _IsNotPristine[nameof(NclobType)] = true;
                _NclobType = value;
            }
        }

        private byte[]? _UuidType;

        [Column("UUID_TYPE"), ColumnType(OracleDbType.Raw)]
        public byte[]? UuidType
        { 
            get
            {
                return _UuidType;
            } 
            set
            {
                _IsNotPristine[nameof(UuidType)] = true;
                _UuidType = value;
            }
        }

        private byte[]? _RawType;

        [Column("RAW_TYPE"), ColumnType(OracleDbType.Raw)]
        public byte[]? RawType
        { 
            get
            {
                return _RawType;
            } 
            set
            {
                _IsNotPristine[nameof(RawType)] = true;
                _RawType = value;
            }
        }

        private decimal _FloatType;

        [Column("FLOAT_TYPE"), ColumnType(OracleDbType.Decimal)]
        public decimal FloatType
        { 
            get
            {
                return _FloatType;
            } 
            set
            {
                _IsNotPristine[nameof(FloatType)] = true;
                _FloatType = value;
            }
        }

        private object _DoubleType;

        [Column("DOUBLE_TYPE"), ColumnType(OracleDbType.Object)]
        public object DoubleType
        { 
            get
            {
                return _DoubleType;
            } 
            set
            {
                _IsNotPristine[nameof(DoubleType)] = true;
                _DoubleType = value;
            }
        }

        private decimal _DecimalType;

        [Column("DECIMAL_TYPE"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalType
        { 
            get
            {
                return _DecimalType;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalType)] = true;
                _DecimalType = value;
            }
        }

        private decimal _DecimalTypeScale0;

        [Column("DECIMAL_TYPE_SCALE_0"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalTypeScale0
        { 
            get
            {
                return _DecimalTypeScale0;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalTypeScale0)] = true;
                _DecimalTypeScale0 = value;
            }
        }

        private decimal _DecimalTypeScale5;

        [Column("DECIMAL_TYPE_SCALE_5"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalTypeScale5
        { 
            get
            {
                return _DecimalTypeScale5;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalTypeScale5)] = true;
                _DecimalTypeScale5 = value;
            }
        }

        private decimal _DecimalTypeScale10;

        [Column("DECIMAL_TYPE_SCALE_10"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalTypeScale10
        { 
            get
            {
                return _DecimalTypeScale10;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalTypeScale10)] = true;
                _DecimalTypeScale10 = value;
            }
        }

        private decimal _DecimalTypeScale15;

        [Column("DECIMAL_TYPE_SCALE_15"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalTypeScale15
        { 
            get
            {
                return _DecimalTypeScale15;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalTypeScale15)] = true;
                _DecimalTypeScale15 = value;
            }
        }

        private decimal _DecimalTypeScale20;

        [Column("DECIMAL_TYPE_SCALE_20"), ColumnType(OracleDbType.Decimal)]
        public decimal DecimalTypeScale20
        { 
            get
            {
                return _DecimalTypeScale20;
            } 
            set
            {
                _IsNotPristine[nameof(DecimalTypeScale20)] = true;
                _DecimalTypeScale20 = value;
            }
        }

        private object _IntervalType;

        [Column("INTERVAL_TYPE"), ColumnType(OracleDbType.Object)]
        public object IntervalType
        { 
            get
            {
                return _IntervalType;
            } 
            set
            {
                _IsNotPristine[nameof(IntervalType)] = true;
                _IntervalType = value;
            }
        }

        private object _IntervalYearType;

        [Column("INTERVAL_YEAR_TYPE"), ColumnType(OracleDbType.Object)]
        public object IntervalYearType
        { 
            get
            {
                return _IntervalYearType;
            } 
            set
            {
                _IsNotPristine[nameof(IntervalYearType)] = true;
                _IntervalYearType = value;
            }
        }

        private object _XmltypeType;

        [Column("XMLTYPE_TYPE"), ColumnType(OracleDbType.Object)]
        public object XmltypeType
        { 
            get
            {
                return _XmltypeType;
            } 
            set
            {
                _IsNotPristine[nameof(XmltypeType)] = true;
                _XmltypeType = value;
            }
        }

        private string? _JsonType;

        [Column("JSON_TYPE"), ColumnType(OracleDbType.Clob)]
        public string? JsonType
        { 
            get
            {
                return _JsonType;
            } 
            set
            {
                _IsNotPristine[nameof(JsonType)] = true;
                _JsonType = value;
            }
        }

    
    
        static SupportedTypesDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
