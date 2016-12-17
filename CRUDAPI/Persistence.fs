module Persistence

open FSharp.Data.Sql
open System

let [<Literal>] private resolutionPath = "..\\..\\..\\packages\\MySql.Data.6.9.9\\lib\\net45\\MySql.Data.dll"
let [<Literal>] connectionString = "Server=localhost;Database=controledecustos;Uid=root;Pwd=123456;"

// create a type alias with the connection string and database vendor settings
type sql = SqlDataProvider< 
              ConnectionString = connectionString,
              DatabaseVendor = Common.DatabaseProviderTypes.MYSQL,
              ResolutionPath = resolutionPath,
              IndividualsAmount = 1000,
              UseOptionTypes = true >
let ctx = sql.GetDataContext()


let insertCentroDeCusto nome codigo =
    let objeto = ctx.Controledecustos.Centrodecusto.Create(codigo, nome)
    objeto.Id <- Guid.NewGuid().ToString()
    ctx.SubmitUpdates()

let insertLancamento (centroId:Guid) valor descricao =
    let objeto = ctx.Controledecustos.Lancamento.Create(centroId.ToString(), DateTime.Now, descricao, valor)
    objeto.Id <- Guid.NewGuid().ToString()
    ctx.SubmitUpdates()

let getCentroDeCusto (centroId:Guid) =
    ctx.Controledecustos.Centrodecusto |> Seq.find (fun cc -> cc.Id = centroId.ToString())

let getLancamento (lancamentoId:Guid) = 
    ctx.Controledecustos.Lancamento |> Seq.find (fun l -> l.Id = lancamentoId.ToString())

let getLancamentosPorCentroDeCusto (centroId:Guid)  = 
    ctx.Controledecustos.Lancamento |> Seq.filter(fun l -> l.CentroDeCustoId = centroId.ToString())

let deleteLancamento (lancamentoId:Guid) =
    let objeto = getLancamento lancamentoId
    objeto.Delete()
    ctx.SubmitUpdates()

let updateLancamento (lancamentoId:Guid) valor =
    let objeto = getLancamento lancamentoId
    objeto.Valor <- valor
    ctx.SubmitUpdates()